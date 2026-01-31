using System.Reflection.Metadata;
using Backend.Data;
using Backend.DTOs;
using Backend.Models;

namespace Backend.Services
{
    public class DocumentService
    {
        private readonly AppDbContext _context;
        

        public DocumentService(AppDbContext context)
        {
            _context = context;
            
        }

        public Document CreateDocument(CreateDocumentDto dto, int userId)
        {
            var document = new Document
            {
                DocumentName = dto.DocumentName,
                DocumentDescription = dto.DocumentDescription,
                DocumentType = dto.DocumentType,
                CreatedBy = userId,
                CreatedOn = DateTime.UtcNow
            };

            _context.Documents.Add(document);
            _context.SaveChanges();

            //  ACTIVITY LOG
            var log = new ActivityLog
            {
                UserId = userId,
                Action = "Document Created",
                EntityName = "Document",
                EntityId = document.DocId,
                CreatedAt = DateTime.UtcNow
            };

            _context.ActivityLogs.Add(log);
            _context.SaveChanges();

            return document;
        }

        public List<Document> GetUserDocuments(int userId)
        {
            return _context.Documents
                .Where(d => d.CreatedBy == userId)
                .OrderByDescending(d => d.CreatedOn)
                .ToList();
        }

        public Document? GetDocumentById(int docId, int userId)
        {
            return _context.Documents
                .FirstOrDefault(d => d.DocId == docId && d.CreatedBy == userId);
        }
    }
}
