using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class DocumentService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly DocumentTextExtractionService _textExtractor;
        private readonly DocumentVersionService _versionService;

        public DocumentService(
            AppDbContext context,
            IWebHostEnvironment env,
            DocumentTextExtractionService textExtractor,
            DocumentVersionService versionService)
        {
            _context = context;
            _env = env;
            _textExtractor = textExtractor;
            _versionService = versionService;
        }

        public Document CreateDocumentWithFile(UploadDocumentDto dto, int userId)
        {
            string? docFolder = null;

            using var tx = _context.Database.BeginTransaction();
            try
            {
                // 1) Create document row
                var document = new Document
                {
                    DocumentName = dto.DocumentName,
                    DocumentDescription = dto.DocumentDescription,
                    DocumentType = dto.DocumentType,
                    UserId = userId,
                    CreatedOn = DateTime.UtcNow
                };

                _context.Documents.Add(document);
                _context.SaveChanges(); // generates DocId

                // 2) Save file to disk
                var uploadsRoot = Path.Combine(_env.ContentRootPath, "Uploads");
                var userFolder = Path.Combine(uploadsRoot, $"user-{userId}");
                docFolder = Path.Combine(userFolder, $"doc-{document.DocId}");
                Directory.CreateDirectory(docFolder);

                var safeFileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.File.FileName)}";
                var physicalPath = Path.Combine(docFolder, safeFileName);

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    dto.File.CopyTo(stream);
                }

                // 3) Save relative path for preview download
                // served by Program.cs mapping "/uploads" => Uploads folder
                document.FilePath = $"uploads/user-{userId}/doc-{document.DocId}/{safeFileName}";
                _context.SaveChanges();

                // 4) Extract text and create Version 1
                string extractedText = "";
                try
                {
                    extractedText = _textExtractor.ExtractText(dto.File);
                }
                catch
                {
                    extractedText = "";
                }

                _versionService.CreateTextVersion(document.DocId, userId, extractedText);

                // 5) Activity log
                _context.ActivityLogs.Add(new ActivityLog
                {
                    UserId = userId,
                    Action = "Document Uploaded",
                    EntityName = "Document",
                    EntityId = document.DocId,
                    CreatedAt = DateTime.UtcNow
                });

                _context.SaveChanges();

                // ✅ commit only after EVERYTHING succeeded
                tx.Commit();
                return document;
            }
            catch
            {
                // ✅ rollback DB changes
                try { tx.Rollback(); } catch { }

                // ✅ delete file/folder if it was created
                try
                {
                    if (!string.IsNullOrWhiteSpace(docFolder) && Directory.Exists(docFolder))
                        Directory.Delete(docFolder, true);
                }
                catch { }

                throw; // let controller return the error
            }
        }

        public List<Document> GetUserDocuments(int userId)
        {
            return _context.Documents
                .Where(d => d.UserId == userId)
                .OrderByDescending(d => d.CreatedOn)
                .ToList();
        }

        public Document? GetDocumentById(int docId, int userId)
        {
            return _context.Documents
                .FirstOrDefault(d => d.DocId == docId && d.UserId == userId);
        }

        public void DeleteDocument(Document document, int userId)
        {
            using var tx = _context.Database.BeginTransaction();

            try
            {
                // 1) Get all version ids for this document
                var versionIds = _context.DocumentVersions
                    .Where(v => v.DocId == document.DocId)
                    .Select(v => v.VersionId)
                    .ToList();

                // 2) Delete inline comments linked to these versions
                var comments = _context.InlineComments
                    .Where(c => versionIds.Contains(c.VersionId))
                    .ToList();

                _context.InlineComments.RemoveRange(comments);

                // 3) Delete versions
                var versions = _context.DocumentVersions
                    .Where(v => v.DocId == document.DocId)
                    .ToList();

                _context.DocumentVersions.RemoveRange(versions);

                // 4) Delete document row
                _context.Documents.Remove(document);

                // 5) Save DB changes first (important for FK)
                _context.SaveChanges();

                // 6) Delete physical folder after DB is safe
                var uploadsRoot = Path.Combine(_env.ContentRootPath, "Uploads");
                var userFolder = Path.Combine(uploadsRoot, $"user-{userId}");
                var docFolder = Path.Combine(userFolder, $"doc-{document.DocId}");

                if (Directory.Exists(docFolder))
                    Directory.Delete(docFolder, true);

                // 7) Activity log
                _context.ActivityLogs.Add(new ActivityLog
                {
                    UserId = userId,
                    Action = "Document Deleted",
                    EntityName = "Document",
                    EntityId = document.DocId,
                    CreatedAt = DateTime.UtcNow
                });

                _context.SaveChanges();

                tx.Commit();
            }
            catch
            {
                try { tx.Rollback(); } catch { }
                throw;
            }
        }

    }
}
