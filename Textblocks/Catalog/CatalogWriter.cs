using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Json;

namespace cwsoft.Textblocks.Catalog;
// Class to write catalog data as serialized gzipped textblocks catalog file (.tbc).
internal static class CatalogWriter
{
   // Serialize catalog data and write gzipped stream to textblocks catalog file (.tbc).
   // Returns boolean status of file operation.
   public static bool Write(string catalogPath, Model.Catalog catalog)
   {
      try {
         using var gzs = new GZipStream(new FileStream(catalogPath, FileMode.Create), CompressionMode.Compress);
         var jsonSerializer = new DataContractJsonSerializer(typeof(Model.Catalog));
         jsonSerializer.WriteObject(gzs, catalog);
         return true;
      }
      catch (Exception) {
         return false;
      }
   }
}
