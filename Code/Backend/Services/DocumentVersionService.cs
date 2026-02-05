using Backend.Data;
using Backend.Models;

namespace Backend.Services
{
    public class DocumentVersionService
    {
        private readonly AppDbContext _context;

        public DocumentVersionService(AppDbContext context)
        {
            _context = context;
        }

        // ===============================
        // CREATE TEXT VERSION (EDITOR)
        // ===============================
        public DocumentVersion CreateTextVersion(int docId, int userId, string text)
        {
            // Determine next version number (safe even if empty)
            int lastVersionNumber = _context.DocumentVersions
                .Where(v => v.DocId == docId)
                .Select(v => (int?)v.VersionNumber)
                .Max() ?? 0;

            var version = new DocumentVersion
            {
                DocId = docId,
                VersionNumber = lastVersionNumber + 1,
                OriginalText = text ?? "",
                UploadedAt = DateTime.UtcNow,
                CreatedById = userId
            };

            // ✅ Save version FIRST
            _context.DocumentVersions.Add(version);
            _context.SaveChanges();

            // ✅ Activity log should NEVER break upload/version creation
            try
            {
                _context.ActivityLogs.Add(new ActivityLog
                {
                    UserId = userId,
                    Action = "Document Version Created",
                    EntityName = "DocumentVersion",
                    EntityId = version.VersionId,
                    CreatedAt = DateTime.UtcNow
                });

                _context.SaveChanges();
            }
            catch
            {
                // swallow logging failure (optional: log to console/ILogger)
            }

            return version;
        }

        // ===============================
        // GET LATEST VERSION
        // ===============================
        public DocumentVersion? GetLatestVersion(int docId)
        {
            return _context.DocumentVersions
                .Where(v => v.DocId == docId)
                .OrderByDescending(v => v.VersionNumber)
                .FirstOrDefault();
        }
    }
}
