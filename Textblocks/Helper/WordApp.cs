using System;
using System.Collections.Generic;

using MSWord = Microsoft.Office.Interop.Word;

namespace cwsoft.Textblocks.Helper;

// Wrapper to automate some basic MS Word tasks using Office Interop services.
// Note: At any time only one active Word document (*.docx) can be opened.
internal class WordApp: IDisposable
{
   #region // Fields, Properties, Constructors
   // Private fields.
   private MSWord.Application? _wordApp = null;

   // Public properties.
   public MSWord.Document? Document { get; private set; } = null;

   // Returns true if MS Word instance exists, otherwise false.
   public bool InstanceExists {
      get {
         try {
            return !string.IsNullOrWhiteSpace(_wordApp?.Version);
         }
         catch (System.Runtime.InteropServices.COMException) {
            return false;
         }
      }
   }

   // Constructor.
   public WordApp(bool visible = false) => CreateInstanceIfNotExists(visible);
   #endregion

   #region // Public API
   // Returns true if specified document is already opened.
   public bool DocumentAlreadyOpen(string fileName) =>
      string.Compare(Document?.FullName ?? string.Empty, fileName, ignoreCase: true) == 0;

   // Returns true if specified Word document was opened, otherwise false.
   // Closes any possible open document to ensure only one document is open at any time.
   public bool OpenDocument(string fileName, bool visible = false, bool readOnly = true)
   {
      // Ensure MS Word connection exists.
      if (!CreateInstanceIfNotExists()) {
         return false;
      }

      // Close possible open document so we have only one open document at any time.
      try {
         _ = CloseDocument(saveChanges: false);
         Document = _wordApp?.Documents.Open(FileName: fileName, ReadOnly: readOnly, Visible: visible);
         return string.Compare(Document?.FullName ?? string.Empty, fileName, ignoreCase: true) == 0;
      }
      catch (System.Runtime.InteropServices.COMException) {
         return false;
      }
   }

   // Close actual opened MS Word document if exist.
   public bool CloseDocument(bool saveChanges = false)
   {
      try {
         Document?.Close(SaveChanges: saveChanges);
      }
      catch (System.Runtime.InteropServices.COMException) {
         return false;
      }
      finally {
         Document = null;
      }
      return true;
   }

   // Remove specified table of contents from actual opened document.
   public void RemoveTableOfContents(int tocIndex = 1)
   {
      try {
         if ((Document?.TablesOfContents.Count ?? 0) > 0) {
            Document?.TablesOfContents[tocIndex].Delete();
         }
      }
      finally { }
   }

   // Return specified Word document property value as string.
   public string GetDocumentProperty(string propertyName, string defaultValue = "")
   {
      if (Document is null) {
         return defaultValue;
      }

      // NOTE: Used dynamic to avoid referencing Microsoft.Office.Core COM-Object library.
      foreach (dynamic property in Document.CustomDocumentProperties) {
         if (property.Name == propertyName) {
            return property.Value.ToString();
         }
      }

      return defaultValue;
   }

   // Iterates over all MS Word range objects of actual catalog matching the given styleName.
   public IEnumerable<MSWord.Range> GetRangesByStyleName(string styleName)
   {
      MSWord.Range? rng = Document?.Range(0, Document.Content?.End ?? -1);
      if (rng is null || Document is null) {
         yield break;
      }

      // Configure MS Word find method to search for styles.
      MSWord.Style style = Document.Styles[styleName];
      rng.Find.ClearFormatting();
      rng.Find.Forward = true;
      rng.Find.Wrap = MSWord.WdFindWrap.wdFindStop;
      rng.Find.Format = true;
      rng.Find.set_Style(style);

      long end = Document.Content?.End ?? -1;
      while (rng.Find.Execute(FindText: "")) {
         if (rng.Start >= end) {
            yield break;
         }
         yield return rng;
      }
   }
   #endregion

   #region // Internal API
   // Create instance of MS Word in background mode. Returns true on success, otherwise false.
   private bool CreateInstanceIfNotExists(bool visible = false)
   {
      if (InstanceExists) {
         return true;
      }

      try {
         _wordApp = new() { Visible = visible };
         return !string.IsNullOrEmpty(_wordApp?.Version);
      }
      catch (System.Runtime.InteropServices.COMException) {
         _wordApp = null;
         return false;
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
      if (!_isDisposed && disposing) {
         _ = CloseDocument();

         if (InstanceExists) {
            // Only dispose _wordApp if no documents are opened to prevent data loss.
            // This may occur, if another MS Word instance is created while Textblocks is running.
            if (_wordApp?.Documents.Count == 0) {
               _wordApp?.Quit(SaveChanges: false);
               _wordApp = null;
            }
            else {
               // Ensure MS Word is visible so user has the chance to close it manually.
               if (_wordApp is not null) {
                  _wordApp.Visible = true;
               }
            }
         }
      }
      _isDisposed = true;
   }
   #endregion
}
