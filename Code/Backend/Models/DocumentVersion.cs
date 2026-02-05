using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class DocumentVersion
    {
        [Key]
        public int VersionId { get; set; }

        // ✅ Single FK
        public int DocId { get; set; }

        // ✅ Tell EF this navigation uses DocId
        [ForeignKey(nameof(DocId))]
        public Document Document { get; set; } = null!;

        public int VersionNumber { get; set; }

        // optional
        public string? FilePath { get; set; }

        public string OriginalText { get; set; } = "";

        public DateTime UploadedAt { get; set; }

        public int CreatedById { get; set; }
        // (optional) If you have User navigation, you can add later safely
        // [ForeignKey(nameof(CreatedById))]
        // public User CreatedBy { get; set; } = null!;
    }
}
