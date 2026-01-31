using Backend.Data;
using Backend.Models;

namespace Backend.Services
{
    public class DocumentVersionService
    {
        private readonly AppDbContext _context;
        private readonly DocumentTextExtractionService _extractor;

        public DocumentVersionService(
            AppDbContext context,
            DocumentTextExtractionService extractor)
        {
            _context = context;
            _extractor = extractor;
        }

        public DocumentVersion CreateVersion(
            int docId,
            int userId,
            IFormFile file)
        {
            // Save file
            var folder = Path.Combine("Uploads", docId.ToString());
            Directory.CreateDirectory(folder);

            var filePath = Path.Combine(folder, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            // Extract text
            var extractedText = _extractor.ExtractText(file);

            // Determine version number
            int versionNumber = _context.DocumentVersions
                .Count(v => v.DocId == docId) + 1;

            var version = new DocumentVersion
            {
                DocId = docId,
                VersionNumber = versionNumber,
                FilePath = filePath,
                OriginalText = extractedText,
                UploadedAt = DateTime.UtcNow,
                CreatedById = userId
            };

            _context.DocumentVersions.Add(version);
            _context.SaveChanges();

            // ACTIVITY LOGGING 
            var log = new ActivityLog
            {
                UserId = userId,
                Action = "Document Version Uploaded",
                EntityName = "DocumentVersion",
                EntityId = version.VersionId,
                CreatedAt = DateTime.UtcNow
            };

            _context.ActivityLogs.Add(log);
            _context.SaveChanges();

            return version;
        }
    }
}
