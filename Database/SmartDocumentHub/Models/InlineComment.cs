using System.ComponentModel.DataAnnotations;

namespace SmartDocumentHub.Models
{
    public class InlineComment
    {
        [Key]
        public int CommentId { get; set; }

        public int StartIndex { get; set; }
        public int EndIndex { get; set; }

        [Required]
        public string CommentText { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // TRUE parent
        public int VersionId { get; set; }
        public DocumentVersion DocumentVersion { get; set; }

        // Author (NO cascade)
        public int AuthorId { get; set; }
        public User Author { get; set; }
    }
}
