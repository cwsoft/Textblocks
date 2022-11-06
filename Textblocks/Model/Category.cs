using System;

namespace cwsoft.Textblocks.Model;

// Data class to store data of a catalog category. 
[Serializable()]
internal class Category
{
   public int Id { get; set; }
   public string Heading { get; set; } = string.Empty;
   public int NbrTextblocksInCategory { get; set; } = 0;

   // String representation.
   public override string ToString() => $"{Id}: {Heading} ({NbrTextblocksInCategory})";
}
