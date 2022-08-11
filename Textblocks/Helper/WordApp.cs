using System;
using System.Collections.Generic;

using MSWord = Microsoft.Office.Interop.Word;

namespace cwsoft.Textblocks.Helper;

// Microsoft Word wrapper to automate some tasks using Office Interop services.
internal class WordApp: IDisposable
{
   #region // Fields, Properties, Constructors.
   // Private fields.
   private MSWord.Application? _wordApp = null;

   // Properties.
   public MSWord.Document? ActiveDocument { get; private set; } = null;

   // Constructors.
   public WordApp(bool visible = false) => Initialize(visible);
   #endregion

   #region // Public API
   // Returns true if document was successfully opened, otherwise false.
   public bool OpenDocument(string fileName, bool visible = false, bool readOnly = true)
   {
      // Ensure Interop service runs and word object is initialized.
      if (!Initialize()) {
         return false;
      }

      // Close actual document so we have only one document open at any time.
      _ = CloseDocument(saveChanges: false);

      // Try to open specified document.
      try {
         ActiveDocument = _wordApp?.Documents.Open(FileName: fileName, ReadOnly: readOnly, Visible: visible);
         return string.Compare(ActiveDocument?.FullName ?? string.Empty, fileName, ignoreCase: true) == 0;
      }
      catch (System.Runtime.InteropServices.COMException) {
         return false;
      }
   }

   // Returns true if active document could be closed, otherwise false.
   public bool CloseDocument(bool saveChanges = false)
   {
      try {
         ActiveDocument?.Close(SaveChanges: saveChanges);
      }
      catch (System.Runtime.InteropServices.COMException) {
         return false;
      }
      finally {
         ActiveDocument = null;
      }
      return true;
   }

   // Remove given table of contents (TOC) from actual opened document.
   public bool RemoveTableOfContents(int tocIndex = 1)
   {
      try {
         if ((ActiveDocument?.TablesOfContents.Count ?? 0) > 0) {
            ActiveDocument?.TablesOfContents[tocIndex].Delete();
            return true;
         }
      }
      catch (Exception) { }

      return false;
   }

   // Returns specified Word document property value as string.
   public string GetDocumentProperty(string propertyName, string defaultValue = "")
   {
      if (ActiveDocument is null) {
         return defaultValue;
      }

      // NOTE: Used dynamic to avoid referencing Microsoft.Office.Core COM-Object library.
      foreach (dynamic property in ActiveDocument.CustomDocumentProperties) {
         if (property.Name == propertyName) {
            return property.Value.ToString();
         }
      }
      return defaultValue;
   }

   // Return specified range from active document if exists or null.
   public MSWord.Range? GetRangeOrDefault(int startPos, int endPos) => ActiveDocument?.Range(startPos, endPos);

   // Iterates over all range objects in actual document matching given styleName.
   public IEnumerable<MSWord.Range> GetRangesByStyleName(string styleName)
   {
      MSWord.Range? rng = ActiveDocument?.Range(0, ActiveDocument.Content?.End ?? -1);
      if (rng is null || ActiveDocument is null) {
         yield break;
      }

      // Configure MS Word find method to search for styles.
      MSWord.Style style = ActiveDocument.Styles[styleName];
      rng.Find.ClearFormatting();
      rng.Find.Forward = true;
      rng.Find.Wrap = MSWord.WdFindWrap.wdFindStop;
      rng.Find.Format = true;
      rng.Find.set_Style(style);

      long end = ActiveDocument.Content?.End ?? -1;
      while (rng.Find.Execute(FindText: "")) {
         if (rng.Start >= end) {
            yield break;
         }
         yield return rng;
      }
   }

   // Returns true if specified document is already open, otherwise false.
   public bool IsDocumentOpen(string fileName) =>
       string.Compare(ActiveDocument?.FullName ?? string.Empty, fileName, ignoreCase: true) == 0;

   // Returns true if word object is initialized and Interop service is running, otherwise false.
   public bool IsInitialized()
   {
      try {
         return !string.IsNullOrWhiteSpace(_wordApp?.Version);
      }
      catch (System.Runtime.InteropServices.COMException) {
         return false;
      }
   }
   #endregion

   #region // Internal API
   // Creates new word object via Interop services and returns true on success, otherwise false.
   private bool Initialize(bool visible = false)
   {
      if (IsInitialized()) {
         return true;
      }

      // Try to initialize word object using Interop services.
      try {
         _wordApp = new() { Visible = visible };
         return IsInitialized();
      }
      catch (System.Runtime.InteropServices.COMException) {
         _ = CloseDocument();
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

         if (IsInitialized()) {
            // Only dispose _wordApp if no documents are opened to prevent data loss.
            // This may occur, if another MS Word instance is created while Textblocks is running.
            if (_wordApp?.Documents.Count == 0) {
               _wordApp?.Quit(SaveChanges: false);
               _wordApp = null;
            }
            else {
               // Ensure MS Word is visible so user can close it manually.
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
