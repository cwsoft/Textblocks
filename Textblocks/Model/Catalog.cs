using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using MSWord = Microsoft.Office.Interop.Word;

namespace cwsoft.Textblocks.Model;

// This class implements the logic to read, extract and write category and textblock
// objects from catalog documents (*.docx) or textblock catalogs (*.tbc).
internal class Catalog: IDisposable
{
   #region // Fields, Properties, Constructors
   // Private fields.
   private readonly Helper.WordApp _wordApp;
   private readonly Control? _infoControl = null;

   // Properties.
   public string DocumentFile { get; private set; } = string.Empty;
   public string CatalogFile { get; private set; } = string.Empty;
   public bool WordInstanceExists => _wordApp.IsInitialized();
   public List<Textblock> Textblocks { get; private set; } = new();
   public List<Category> Categories { get; private set; } = new();

   // Access text property of optional form control element.
   public string InfoText {
      get => _infoControl?.Text ?? string.Empty;
      private set {
         if (_infoControl is not null) {
            _infoControl.Text = value;
         }
      }
   }

   // Returns true if textblock catalog file (*.tbc) exists and is up to date, otherwise false.
   public bool IsCatalogFileUpToDate => File.Exists(DocumentFile)
      && File.Exists(CatalogFile)
      && File.GetLastWriteTime(CatalogFile) > File.GetLastWriteTime(DocumentFile);

   // Constructor.
   public Catalog(Helper.WordApp wordApp, Control? infoControl = null)
      => (_wordApp, _infoControl) = (wordApp, infoControl);
   #endregion

   #region // Public API
   // Returns true if catalog document (*.docx) exists, otherwise false.
   public static bool IsValidDocumentFile(string fileName) => File.Exists(fileName)
      && Path.GetExtension(fileName).EndsWith(".docx", StringComparison.OrdinalIgnoreCase);

   // Open specified catalog document (*.docx) in MS Word if not already open.
   public bool OpenCatalog(string fileName = "", bool allowCatalogSelection = true)
   {
      // Let user select a valid catalog document (*.docx) if given filename is invalid.
      if (allowCatalogSelection && !IsValidDocumentFile(fileName)) {
         fileName = SelectCatalogDocumentFile();
      }

      // Validate filename and set internal pathes to catalog files (*.tbc, *.docx).
      if (!SetValidatedCatalogPathes(fileName)) {
         return false;
      }

      // Check if catalog document is already opened in MS Word.
      if (_wordApp.IsDocumentOpen(DocumentFile) && MessageBox.Show(
         "Die Katalogdatei ist bereits geladen.\nMöchten Sie die Katalogdatei erneut laden?",
         "Katalogdatei (*.docx) bereits geladen",
         MessageBoxButtons.YesNo,
         MessageBoxIcon.Question) == DialogResult.No) {
         return false;
      }

      // Open catalog document (*.docx) in MS Word and remove TOC for faster data extraction on large catalogs.
      if (_wordApp.OpenDocument(DocumentFile, visible: false, readOnly: true)) {
         _wordApp.RemoveTableOfContents(tocIndex: 1);
         return true;
      }

      return false;
   }

   // Extract categories and textblocks from selected catalog. Returns true on success, otherwise false.
   public bool ExtractCatalog()
   {
      ResetCatalog();

      // Only proceed if corresponding catalog document (*.docx) is opened in MS Word.
      if (!(_wordApp.IsDocumentOpen(fileName: DocumentFile))) {
         return false;
      }

      // Prefer extraction from textblock catalog file (*.tbc) for speed.
      if (IsCatalogFileUpToDate && ReadCatalogFile()) {
         return true;
      }

      // Extract from catalog document (*.docx) as fallback.
      try {
         if (ExtractFromDocumentFile()) {
            // Create textblock catalog file (*.tbc) to speed up next loading.
            _ = WriteCatalogFile();
            return true;
         }
         return false;
      }
      catch (Exception) {
         ResetCatalog();
         return false;
      }
   }

   // Close actual catalog document (*.docx) in MS Word and clear Categories and Textblocks list.
   public void CloseCatalog()
   {
      _ = _wordApp.CloseDocument();
      ResetCatalog();
   }

   // Return category object defined by it's Id from actual loaded categories or null.
   public Category? GetCategoryById(int id) => Categories.Where(x => x.Id == id).FirstOrDefault();

   // Return list of textblocks matching specified category id (category id:=0 => all categories).
   public List<Textblock> GetTextblocksByCategoryId(int categoryId = 0)
      => (categoryId == 0) ? Textblocks : Textblocks.Where(x => x.CategoryId == categoryId).ToList();

   // Return textblock object defined by it's Id from the actual loaded textblocks or null.
   public Textblock? GetTextblockById(int id) => Textblocks.Where(x => x.Id == id).FirstOrDefault();

   // Returns MS Word document range of given textblock or null.
   public MSWord.Range? GetTextblockDocumentRange(Textblock textblock)
   {
      if (textblock is null) {
         return null;
      }
      return _wordApp.GetRangeOrDefault(textblock.RngStartPos, textblock.RngEndPos);
   }
   #endregion

   #region // Internal API
   // Reset catalog into defined state.
   private void ResetCatalog()
   {
      Categories.Clear();
      Textblocks.Clear();
   }

   // Return selected catalog document or empty string in case selection was canceled.
   private static string SelectCatalogDocumentFile()
   {
      // Show file open dialog to select a valid catalog document (*.docx).
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

      string documentFile = (dialog.ShowDialog() == DialogResult.OK) ? dialog.FileName : string.Empty;
      return IsValidDocumentFile(documentFile) ? documentFile : string.Empty;
   }

   // Validate catalog document filename and set internal catalog pathes (.docx, .tbc). 
   // Returns true if filename is valid and pathes are set, otherwise false.
   private bool SetValidatedCatalogPathes(string fileName)
   {
      if (IsValidDocumentFile(fileName)) {
         DocumentFile = Path.GetFullPath(fileName);
         CatalogFile = $@"{Path.GetDirectoryName(DocumentFile)}" + $@"\{Path.GetFileNameWithoutExtension(DocumentFile)}.tbc";
         return true;
      }

      (DocumentFile, CatalogFile) = (string.Empty, string.Empty);
      return false;
   }

   // Extract categories and textblocks from catalog document (*.docx).
   // Returns true on success, otherwise false.
   private bool ExtractFromDocumentFile()
   {
      // Extract style names from document properties or use defaults.
      string categoryStyleName = _wordApp.GetDocumentProperty("categoryStyleName", Properties.Resources.DefaultCategoryStyleName);
      string textblockStyleName = _wordApp.GetDocumentProperty("textblockStyleName", Properties.Resources.DefaultTextblockStyleName);

      // Extract catalog data.
      (int nbrCategories, int nbrTextblocks, int documentEnd) = (0, 0, _wordApp.ActiveDocument?.Content?.End ?? -1);
      foreach (MSWord.Range? rng in _wordApp.GetRangesByStyleName(textblockStyleName)) {
         if (rng is null) {
            return false;
         }

         double completed = rng.Start / (double) documentEnd * 100;
         InfoText = $"Extrahiere Katalogdaten aus Word-Datei '{Path.GetFileName(DocumentFile)}' ... Status [{completed:F0}%]";

         // Extract range and style name located just before actual textblock.
         MSWord.Range? previousRange = rng.Paragraphs[1]?.Previous(1)?.Range;
         if (previousRange is null) {
            return false;
         }
         string previousStyleName = ((MSWord.Style) previousRange.get_Style()).NameLocal;

         if (previousStyleName == categoryStyleName) {
            Categories.Add(new() {
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

            Textblocks[nbrTextblocks - 1].RngEndPos = previousTextblockEnd;
            Textblocks[nbrTextblocks - 1].Content =
               _wordApp.ActiveDocument?.Range(Textblocks[nbrTextblocks - 1].RngStartPos, previousTextblockEnd).Text ?? string.Empty;
         }

         // Add all available information of actual textblock.
         Textblocks.Add(new() {
            Id = ++nbrTextblocks,
            CategoryId = nbrCategories,
            Heading = rng.Text,
            RngStartPos = rng.Start
         });
      }

      // Update missing RngEndPos and Content of very last textblock.
      if (Textblocks.Count > 0) {
         Textblocks[nbrTextblocks - 1].RngEndPos = documentEnd;
         Textblocks[nbrTextblocks - 1].Content =
            _wordApp.ActiveDocument?.Range(Textblocks[nbrTextblocks - 1].RngStartPos, documentEnd).Text ?? string.Empty;
      }

      // Add number of available textblocks for each category.
      for (int i = 0; i < Categories.Count; i++) {
         Categories[i].NbrTextblocksInCategory = GetTextblocksByCategoryId(i + 1).Count;
      }

      // Add summary category containg all available textblocks.
      Categories.Insert(0, new() {
         Id = 0,
         Heading = "Alle Kategorien",
         NbrTextblocksInCategory = nbrTextblocks
      });
      return true;
   }

   // Write actual Categories and Textblocks objects to textblock catalog file (*.tbc).
   // Returns true on success, otherwise false.
   private bool WriteCatalogFile()
   {
      var catalogFile = new CatalogFile(Categories, Textblocks);
      return catalogFile.WriteCatalog(CatalogFile);
   }

   // Read textblock catalog file (*.tbc) and update actual Categories and Textblocks.
   // Returns true on success, otherwise false.
   private bool ReadCatalogFile()
   {
      var catalogFile = new CatalogFile();
      (Categories, Textblocks) = catalogFile.ReadCatalog(CatalogFile);
      return (Categories.Count > 0 && Textblocks.Count > 0);
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
