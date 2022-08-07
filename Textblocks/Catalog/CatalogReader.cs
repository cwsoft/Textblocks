using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Json;

namespace cwsoft.Textblocks.Catalog;
// Class to read catalog data from serialized gzipped textblocks catalog file (.tbc).
internal class CatalogReader
{
   private Model.CatalogData _catalogData = new();

   // Constructors.
   public CatalogReader() { }
   public CatalogReader(string catalogPath) => Read(catalogPath);

   // Deconstruction.
   public void Deconstruct(out Model.CatalogData catalogData) => (catalogData) = (_catalogData);

   // Read data from serialized gzipped textblocks catalog file (.tbc) and return catalog data object.
   public (bool, Model.CatalogData) Read(string catalogPath)
   {
      try {
         using var gzs = new GZipStream(new FileStream(catalogPath, FileMode.Open), CompressionMode.Decompress);
         var jsonSerializer = new DataContractJsonSerializer(typeof(Model.CatalogData));
         _catalogData = (Model.CatalogData) (jsonSerializer?.ReadObject(gzs) ??
            throw new IOException($"Katalogdatei '{catalogPath}' konnte nicht geladen werden."));
         return (true, _catalogData);
      }
      catch (Exception) {
         return (false, new());
      }
   }
}
