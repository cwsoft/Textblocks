using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Json;

namespace cwsoft.Textblocks.Model;

// Class to read and write Category and Textblock objects from or to textblock catalog files (.tbc).
[Serializable()]
internal class CatalogFile
{
   #region // Fields, Properties, Constructors
   // Private fields.
   private List<Category> _categories;
   private List<Textblock> _textblocks;

   // Constructors.
   public CatalogFile() => (_categories, _textblocks) = (new(), new());

   public CatalogFile(List<Category> categories, List<Textblock> textblocks)
      => (_categories, _textblocks) = (categories, textblocks);

   // Deconstruction.
   public void Deconstruct(out List<Category> categories, out List<Textblock> textblocks)
      => (categories, textblocks) = (_categories, _textblocks);
   #endregion

   #region // Public API
   // Write Category and Textblock objects of this instance to specified textblock catalog (.tbc).
   // Return true on success, otherwise false.
   public bool WriteCatalog(string fileName)
   {
      try {
         using var gzs = new GZipStream(new FileStream(fileName, FileMode.Create), CompressionMode.Compress);
         var jsonSerializer = new DataContractJsonSerializer(typeof(CatalogFile));
         jsonSerializer.WriteObject(gzs, this);
         return true;
      }
      catch (Exception) {
         return false;
      }
   }

   // Read specified textblock catalog (.tbc) and store Catalog and Textblock objects in actual instance.
   public CatalogFile ReadCatalog(string fileName)
   {
      try {
         using var gzs = new GZipStream(new FileStream(fileName, FileMode.Open), CompressionMode.Decompress);
         var jsonSerializer = new DataContractJsonSerializer(typeof(CatalogFile));
         (_categories, _textblocks) = (CatalogFile) (jsonSerializer?.ReadObject(gzs) ?? throw new IOException($"Katalogdatei '{fileName}' konnte nicht geladen werden."));
         return this;
      }
      catch (Exception) {
         return new();
      }
   }
   #endregion
}
