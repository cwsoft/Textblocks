using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Json;

namespace cwsoft.Textblocks.Catalog;
// Class to write catalog data as serialized gzipped textblocks catalog file (.tbc).
internal class CatalogWriter
{
   // Constructors.
   public CatalogWriter() { }
   public CatalogWriter(string catalogPath, Model.CatalogData catalogData) => Write(catalogPath, catalogData);

   // Serialize catalog data and write gzipped stream to textblocks catalog file (.tbc).
   public bool Write(string catalogPath, Model.CatalogData catalogData)
   {
      try {
         using var gzs = new GZipStream(new FileStream(catalogPath, FileMode.Create), CompressionMode.Compress);
         var jsonSerializer = new DataContractJsonSerializer(typeof(Model.CatalogData));
         jsonSerializer.WriteObject(gzs, catalogData);
         return true;
      }
      catch (Exception) {
         return false;
      }
   }
}
