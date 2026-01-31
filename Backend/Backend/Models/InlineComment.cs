using System.ComponentModel.DataAnnotations;

namespace Backend.Models
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

        public int VersionId { get; set; }
        public DocumentVersion DocumentVersion { get; set; }

        public int AuthorId { get; set; }
        public User Author { get; set; }
    }

}
