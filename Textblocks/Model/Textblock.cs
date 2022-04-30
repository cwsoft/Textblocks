using System;

namespace cwsoft.Textblocks.Model;

// Data class to store data of a catalog textblock. 
[Serializable()]
public class Textblock
{
   public int Id { get; set; }
   public int CategoryId { get; set; }
   public string Heading { get; set; } = string.Empty;
   public string Content { get; set; } = string.Empty;
   public int RngStartPos { get; set; }
   public int RngEndPos { get; set; }

   // String representation.
   public override string ToString() => $"{Id}: {Heading}";
}
