﻿using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Json;

namespace cwsoft.Textblocks.Catalog;
// Class to read catalog data from serialized gzipped textblocks catalog file (.tbc).
internal static class CatalogReader
{
   // Read data from serialized gzipped textblocks catalog file (.tbc).
   // Returns Tuple with file operation status and the catalog data object.
   public static (bool, Model.CatalogData) Read(string catalogPath)
   {
      try {
         using var gzs = new GZipStream(new FileStream(catalogPath, FileMode.Open), CompressionMode.Decompress);
         var jsonSerializer = new DataContractJsonSerializer(typeof(Model.CatalogData));
         var catalogData = (Model.CatalogData) (jsonSerializer?.ReadObject(gzs) ??
            throw new IOException($"Katalogdatei '{catalogPath}' konnte nicht geladen werden."));
         return (true, catalogData);
      }
      catch (Exception) {
         return (false, new());
      }
   }
}
