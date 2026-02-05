using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class ActivityLog
    {
        [Key]
        public int ActivityId { get; set; }

        [Required]
        public string Action { get; set; }

        public string EntityName { get; set; }
        public int? EntityId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }
        public User User { get; set; }
    }

}
