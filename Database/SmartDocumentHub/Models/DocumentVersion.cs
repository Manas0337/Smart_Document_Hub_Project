using System.ComponentModel.DataAnnotations;

namespace SmartDocumentHub.Models
{
    public class DocumentVersion
    {
        [Key]
        public int VersionId { get; set; }

        public int VersionNumber { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public string FilePath { get; set; }

        // TRUE parent
        public int DocId { get; set; }
        public Document Document { get; set; }

        // Audit only
        public int CreatedById { get; set; }
        public User CreatedBy { get; set; }

        public ICollection<InlineComment> InlineComments { get; set; }
    }
}
