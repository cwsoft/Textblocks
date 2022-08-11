using System;
using System.Collections.Generic;
using System.IO;

// Class holding relevant data of a Textblock catalog.
namespace cwsoft.Textblocks.Model;
[Serializable()]
internal class CatalogData
{
   #region // Properties, Constructors.
   // Public properties.
   public string DocumentPath { get; private set; }
   public List<Category> Categories { get; private set; }
   public List<Textblock> Textblocks { get; private set; }

   // Constructors.
   public CatalogData() : this(string.Empty, new(), new()) { }
   public CatalogData(string documentPath) : this(documentPath, new(), new()) { }
   public CatalogData(string documentPath, List<Category> categories, List<Textblock> textblocks)
      => (DocumentPath, Categories, Textblocks) = (documentPath, categories, textblocks);
   #endregion

   // String representation of catalog data object.
   public override string ToString()
   {
      try {
         return $"Katalog: '{Path.GetFileName(DocumentPath)}', erstellt am: {File.GetLastWriteTime(DocumentPath)}" +
            $", Kategorien: {Categories.Count}, Textblöcke: {Textblocks.Count}";
      }
      catch (Exception) {
         return $"Katalog: '-', Kategorien: {Categories.Count}, Textblöcke: {Textblocks.Count}";
      }
   }
}
