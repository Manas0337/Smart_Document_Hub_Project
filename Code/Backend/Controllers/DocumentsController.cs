using Backend.DTOs;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/documents")]
    public class DocumentsController : ControllerBase
    {
        private readonly DocumentService _documentService;
        private readonly DocumentVersionService _versionService;

        public DocumentsController(
            DocumentService documentService,
            DocumentVersionService versionService)
        {
            _documentService = documentService;
            _versionService = versionService;
        }

        // ===============================
        // CREATE DOCUMENT
        // ===============================
        [HttpPost]
        [Consumes("multipart/form-data")]
        public IActionResult Create([FromForm] UploadDocumentDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.DocumentName))
                return BadRequest("Document name is required");

            if (dto.File == null || dto.File.Length == 0)
                return BadRequest("File is required");

            // ✅ SAFELY READ USER ID FROM TOKEN
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Console.WriteLine("NameIdentifier claim = " + userIdStr);

            if (!int.TryParse(userIdStr, out int userId))
                return Unauthorized("Invalid or missing user id in token");

            try
            {
                var document = _documentService.CreateDocumentWithFile(dto, userId);

                return Ok(new
                {
                    document.DocId,
                    document.DocumentName,
                    document.DocumentType,
                    document.CreatedOn
                });
            }
            catch (Exception ex)
            {
                // ✅ show real error (your middleware will also show in development)
                return StatusCode(500, ex.Message);
            }
        }

        // ===============================
        // GET USER DOCUMENTS
        // ===============================
        [HttpGet]
        public IActionResult GetMyDocuments()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId))
                return Unauthorized("Invalid or missing user id in token");

            var documents = _documentService.GetUserDocuments(userId)
                .Select(d => new DocumentResponseDto
                {
                    DocId = d.DocId,
                    DocumentName = d.DocumentName,
                    DocumentDescription = d.DocumentDescription,
                    DocumentType = d.DocumentType,
                    CreatedOn = d.CreatedOn,
                    FilePath = d.FilePath
                });

            return Ok(documents);
        }

        // ===============================
        // GET DOCUMENT BY ID
        // ===============================
        [HttpGet("{docId}")]
        public IActionResult GetById(int docId)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId))
                return Unauthorized("Invalid or missing user id in token");

            var document = _documentService.GetDocumentById(docId, userId);

            if (document == null)
                return NotFound("Document not found or access denied");

            return Ok(new DocumentResponseDto
            {
                DocId = document.DocId,
                DocumentName = document.DocumentName,
                DocumentDescription = document.DocumentDescription,
                DocumentType = document.DocumentType,
                CreatedOn = document.CreatedOn,
                FilePath = document.FilePath
            });
        }

        // ===============================
        // DELETE DOCUMENT
        // ===============================
        [HttpDelete("{docId}")]
        public IActionResult Delete(int docId)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId))
                return Unauthorized("Invalid or missing user id in token");

            var document = _documentService.GetDocumentById(docId, userId);

            if (document == null)
                return NotFound("Document not found");

            _documentService.DeleteDocument(document, userId);

            return NoContent(); // 204
        }

        // ===============================
        // PREVIEW DOCUMENT (TEXT)
        // ===============================
        [HttpGet("{docId}/preview")]
        public IActionResult Preview(int docId)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId))
                return Unauthorized("Invalid or missing user id in token");

            // Validate document ownership
            var document = _documentService.GetDocumentById(docId, userId);
            if (document == null)
                return NotFound("Document not found");

            // Get latest version
            var latestVersion = _versionService.GetLatestVersion(docId);
            if (latestVersion == null)
                return NotFound("No preview available");

            return Ok(new
            {
                docId = document.DocId,
                documentName = document.DocumentName,
                version = latestVersion.VersionNumber,
                content = latestVersion.OriginalText
            });
        }
    }
}
