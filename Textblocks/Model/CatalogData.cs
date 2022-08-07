using System;
using System.Collections.Generic;

// Class holding relevant data of a Textblock catalog.
namespace cwsoft.Textblocks.Model;
[Serializable()]
internal class CatalogData
{
   #region // Fields, Properties, Constructors
   // Private fields.
   private readonly string _catalogPath = string.Empty;
   private readonly List<Category> _categories = new();
   private readonly List<Textblock> _textblocks = new();

   // Public properties.
   public string CatalogPath => _catalogPath;
   public List<Category> Categories => _categories;
   public List<Textblock> Textblocks => _textblocks;

   // Constructors.
   public CatalogData() : this(catalogPath: string.Empty, categories: new(), textblocks: new()) { }

   public CatalogData(string catalogPath, List<Category> categories, List<Textblock> textblocks)
      => (_catalogPath, _categories, _textblocks) = (catalogPath, categories, textblocks);

   // Deconstruction.
   public void Deconstruct(out string catalogPath, out List<Category> categories, out List<Textblock> textblocks)
      => (catalogPath, categories, textblocks) = (CatalogPath, Categories, Textblocks);
   #endregion

   // String representation.
   public override string ToString()
      => $"{CatalogPath}, geändert am: TIMESTAMP, Kategorien: {Categories.Count}, Textblöcke: {Textblocks.Count}";
}
