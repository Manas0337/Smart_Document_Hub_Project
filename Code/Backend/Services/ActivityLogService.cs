using Backend.Data;
using Backend.Models;

namespace SmartDocs.API.Services
{
    public class ActivityLogService
    {
        private readonly AppDbContext _context;

        public ActivityLogService(AppDbContext context)
        {
            _context = context;
        }

        public void Log(
            int userId,
            string action,
            string? entityName = null,
            int? entityId = null)
        {
            var log = new ActivityLog
            {
                UserId = userId,
                Action = action,
                EntityName = entityName,
                EntityId = entityId,
                CreatedAt = DateTime.UtcNow
            };

            _context.ActivityLogs.Add(log);
            _context.SaveChanges();
        }
    }
}
