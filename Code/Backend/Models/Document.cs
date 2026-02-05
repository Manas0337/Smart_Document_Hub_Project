using Backend.Models;
using System.ComponentModel.DataAnnotations;

public class Document
{
    [Key]
    public int DocId { get; set; }

    public string DocumentName { get; set; }
    public string? DocumentDescription { get; set; }
    public string DocumentType { get; set; }

    public DateTime CreatedOn { get; set; }

    public string? FilePath { get; set; }   // REQUIRED for storing locally
    
    // ✅ Ownership
    public int UserId { get; set; }
    public User User { get; set; }

    public ICollection<DocumentVersion> Versions { get; set; }
}
