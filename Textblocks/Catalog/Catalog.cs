using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using MSWord = Microsoft.Office.Interop.Word;

namespace cwsoft.Textblocks.Catalog;

// Class to open catalog documents (.docx) in MS Word and to store relevant data as catalog data object in memory.
// Extracted data is stored in JSON serialized gzipped catalog files (.tbc) to speed up subsequent catalog loading.
internal class Catalog: IDisposable
{
   #region // Fields, Properties, Constructors
   // Private fields.
   private readonly Helper.WordApp _wordApp;
   private readonly Control? _infoControl = null;

   // Properties.
   public bool IsInitialized => _wordApp?.IsInitialized() ?? false;
   public Model.CatalogData Data { get; private set; } = new();

   // Access text property of optional form control element.
   public string InfoText {
      get => _infoControl?.Text ?? string.Empty;
      private set {
         if (_infoControl?.Text is not null) {
            _infoControl.Text = value;
         }
      }
   }

   // Constructor.
   public Catalog(Control? infoControl = null)
   {
      _wordApp = new(visible: false);
      _infoControl = infoControl;
   }
   #endregion

   #region // Public API
   // Returns path of catalog file (.docx) selected via file open dialog or empty string.
   private static string SelectCatalog()
   {
      var dialog = new OpenFileDialog {
         Title = "Öffne Textblocks Katalog",
         InitialDirectory = File.Exists(Properties.Settings.Default.LastUsedCatalog)
            ? Path.GetDirectoryName(Properties.Settings.Default.LastUsedCatalog)
            : Application.StartupPath,
         CheckPathExists = true,
         CheckFileExists = true,
         ReadOnlyChecked = true,
         DefaultExt = ".docx",
         Filter = "Textblock Katalog|*.docx"
      };

      return dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : string.Empty;
   }

   // Open document file (*.docx) of specified catalog in Word as ActiveDocument and extract catalog data.
   public bool OpenCatalog(string catalogPath, bool allowCatalogSelection = true)
   {
      // Let user select a Word catalog (.docx) if needed.
      if (allowCatalogSelection && string.IsNullOrEmpty(catalogPath)) {
         catalogPath = SelectCatalog();
      }

      // Build pathes to catalog file (.tbc) and corresponding document file (.docx).
      catalogPath = GetCatalogPathByExtension(catalogPath, ".tbc");
      string documentPath = GetCatalogPathByExtension(catalogPath, ".docx");

      // Only proceed if a valid catalog document (.docx) exists.
      if (!(File.Exists(documentPath) && Path.GetExtension(documentPath).EndsWith(".docx", StringComparison.OrdinalIgnoreCase))) {
         return false;
      }

      // Open catalog document (.docx) in Word as ActiveDocument.
      if (_wordApp.OpenDocument(documentPath)) {
         // Remove TOC from catalog document (.docx) to speed up data extraction.
         _ = _wordApp.RemoveTableOfContents(tocIndex: 1);

         // Read catalog data from catalog file (.tbc) if exists.
         bool status;
         if (File.Exists(catalogPath) && File.GetLastWriteTime(catalogPath) > File.GetLastWriteTime(documentPath)) {
            (status, Data) = CatalogReader.Read(catalogPath);
            if (status) {
               return Data.Categories.Count > 0 && Data.Textblocks.Count > 0;
            }
         }

         // Extract catalog data from ActiveDocument(.docx) if no valid catalog file (.tbc) yet exists.
         (status, Data) = ExtractCatalogDataFromDocument(documentPath);
         if (status) {
            // Write catalog data as serialized gzipped catalog file (*.tbc) to speed up subsequent loading.
            _ = CatalogWriter.Write(catalogPath, Data);
            return Data.Categories.Count > 0 && Data.Textblocks.Count > 0;
         }
      }

      return false;
   }

   // Reset catalog object into defined state.
   public void CloseCatalog()
   {
      _ = _wordApp.CloseDocument();
      Data = new();
      InfoText = "Bitte gültige Katalogdatei öffnen ('Datei -> Katalog öffnen') oder das Program beenden.";
   }

   // Returns category object from Categories matching given Id or null.
   public Model.Category? GetCategoryById(int id) => Data.Categories.Where(x => x.Id == id).FirstOrDefault();

   // Returns textblock object from Textblocks matching given Id or null.
   public Model.Textblock? GetTextblockById(int id) => Data.Textblocks.Where(x => x.Id == id).FirstOrDefault();

   // Returns list of textblocks matching given CategoryId (categoryId:=0 => all categories).
   public List<Model.Textblock> GetTextblocksByCategoryId(int categoryId = 0)
      => (categoryId == 0) ? Data.Textblocks : Data.Textblocks.Where(x => x.CategoryId == categoryId).ToList();

   // Returns MS Word document range of given textblock or null.
   public MSWord.Range? GetTextblockDocumentRange(Model.Textblock textblock)
   {
      if (textblock is not null) {
         return _wordApp.GetRangeOrDefault(textblock.RngStartPos, textblock.RngEndPos);
      }
      return null;
   }
   #endregion

   #region // Private API
   // Extract data from catalog document (.docx) or read from catalog file (.tbc) and hold data as catalog data object in memory.
   private (bool, Model.CatalogData) ExtractCatalogDataFromDocument(string documentPath)
   {
      // Create clean catalog data object for new data extraction.
      var catalogData = new Model.CatalogData(documentPath);

      // Extract style names from document properties or use defaults.
      string categoryStyleName = _wordApp.GetDocumentProperty("categoryStyleName", Properties.Resources.DefaultCategoryStyleName);
      string textblockStyleName = _wordApp.GetDocumentProperty("textblockStyleName", Properties.Resources.DefaultTextblockStyleName);

      // Extract catalog data.
      (int nbrCategories, int nbrTextblocks, int documentEnd) = (0, 0, _wordApp.ActiveDocument?.Content?.End ?? -1);
      foreach (MSWord.Range? rng in _wordApp.GetRangesByStyleName(textblockStyleName)) {
         if (rng is null) {
            return (false, catalogData);
         }

         double completed = rng.Start / (double) documentEnd * 100;
         InfoText = $"Extrahiere Katalogdaten aus Word-Datei '{Path.GetFileName(documentPath)}' ... Status [{completed:F0}%]";

         // Extract range and style name located just before actual textblock.
         MSWord.Range? previousRange = rng.Paragraphs[1]?.Previous(1)?.Range;
         if (previousRange is null) {
            return (false, catalogData);
         }
         string previousStyleName = ((MSWord.Style) previousRange.get_Style()).NameLocal;

         if (previousStyleName == categoryStyleName) {
            catalogData.Categories.Add(new() {
               Id = ++nbrCategories,
               Heading = previousRange.Text
            });
         }

         // Update missing RngEndPos and Content of previous textblock.
         if (nbrTextblocks > 0) {
            // Previous range supposed to be actual category or end of previous textblock.
            int previousTextblockEnd = (previousStyleName == categoryStyleName)
               ? previousRange.Paragraphs[1].Previous(1).Range.End
               : previousRange.End;

            catalogData.Textblocks[nbrTextblocks - 1].RngEndPos = previousTextblockEnd;
            catalogData.Textblocks[nbrTextblocks - 1].Content =
               _wordApp.ActiveDocument?.Range(catalogData.Textblocks[nbrTextblocks - 1].RngStartPos, previousTextblockEnd).Text ?? string.Empty;
         }

         // Add all available information of actual textblock.
         catalogData.Textblocks.Add(new() {
            Id = ++nbrTextblocks,
            CategoryId = nbrCategories,
            Heading = rng.Text,
            RngStartPos = rng.Start
         });
      }

      // Update missing RngEndPos and Content of very last textblock.
      if (catalogData.Textblocks.Count > 0) {
         catalogData.Textblocks[nbrTextblocks - 1].RngEndPos = documentEnd;
         catalogData.Textblocks[nbrTextblocks - 1].Content =
            _wordApp.ActiveDocument?.Range(catalogData.Textblocks[nbrTextblocks - 1].RngStartPos, documentEnd).Text ?? string.Empty;
      }

      // Add number of available textblocks for each category.
      for (int i = 0; i < catalogData.Categories.Count; i++) {
         catalogData.Categories[i].NbrTextblocksInCategory = GetTextblocksByCategoryId(i + 1).Count;
      }

      // Add summary category containg all available textblocks.
      catalogData.Categories.Insert(0, new() {
         Id = 0,
         Heading = "Alle Kategorien",
         NbrTextblocksInCategory = nbrTextblocks
      });

      return (true, catalogData);
   }

   // Returns path to catalog data file (.tbc) or catalog document (.docx) depending on given extension.
   private static string GetCatalogPathByExtension(string catalogPath, string extension = ".docx")
   {
      try {
         extension = extension.StartsWith(".", StringComparison.CurrentCultureIgnoreCase) ? extension : $".{extension}";
         return $@"{Path.GetDirectoryName(catalogPath)}" + $@"\{Path.GetFileNameWithoutExtension(catalogPath)}{extension}";
      }
      catch (Exception) {
         return string.Empty;
      }
   }
   #endregion

   #region // IDisposable Support
   private bool _isDisposed = false;

   // Implements IDisposable interface.
   public void Dispose() => Dispose(true);

   // Free references to MS Word application and MS document.
   protected virtual void Dispose(bool disposing)
   {
      if (!_isDisposed) {
         if (disposing) {
            _wordApp.Dispose();
         }
         _isDisposed = true;
      }
   }
   #endregion
}
