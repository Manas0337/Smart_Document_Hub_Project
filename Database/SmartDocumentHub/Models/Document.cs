using System.ComponentModel.DataAnnotations;

namespace SmartDocumentHub.Models
{
    public class Document
    {
        [Key]
        public int DocId { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }
        public string Category { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Owner (NO cascade)
        public int OwnerId { get; set; }
        public User Owner { get; set; }

        // TRUE ownership
        public ICollection<DocumentVersion> Versions { get; set; }
    }
}
