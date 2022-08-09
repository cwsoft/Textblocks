using System;
using System.Collections.Generic;
using System.IO;

// Class holding relevant data of a Textblock catalog.
namespace cwsoft.Textblocks.Model;
[Serializable()]
internal class CatalogData
{
   #region // Fields, Properties, Constructors.
   // Private fields.
   private readonly string _catalogPath = string.Empty;
   private readonly List<Category> _categories = new();
   private readonly List<Textblock> _textblocks = new();

   // Public properties.
   public string CatalogPath => GetCatalogPathByExtension(".tbc");
   public string DocumentPath => GetCatalogPathByExtension(".docx");
   public List<Category> Categories => _categories;
   public List<Textblock> Textblocks => _textblocks;

   // Constructors.
   public CatalogData() : this(string.Empty, new(), new()) { }

   public CatalogData(string catalogPath, List<Category> categories, List<Textblock> textblocks)
      => (_catalogPath, _categories, _textblocks) = (catalogPath, categories, textblocks);
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

   // Returns path to catalog file (.tbc) or catalog document file (.docx) or empty string.
   private string GetCatalogPathByExtension(string extension = ".tbc")
   {
      try {
         extension = extension.StartsWith(".", StringComparison.CurrentCultureIgnoreCase) ? extension : $".{extension}";
         return $@"{Path.GetDirectoryName(_catalogPath)}" + $@"\{Path.GetFileNameWithoutExtension(_catalogPath)}{extension}";
      }
      catch (Exception) {
         return string.Empty;
      }
   }
}
