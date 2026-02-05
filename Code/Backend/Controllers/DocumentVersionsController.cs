using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers
{
    public class CreateVersionDto
    {
        public string Text { get; set; } = "";
    }

    [Authorize]
    [ApiController]
    [Route("api/documents/{docId}/versions")]
    public class DocumentVersionsController : ControllerBase
    {
        private readonly DocumentVersionService _versionService;

        public DocumentVersionsController(DocumentVersionService versionService)
        {
            _versionService = versionService;
        }

        [HttpPost]
        public IActionResult CreateVersion(int docId, [FromBody] CreateVersionDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Text))
                return BadRequest("Text content is required");

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var version = _versionService.CreateTextVersion(docId, userId, dto.Text);

            return Ok(new
            {
                versionId = version.VersionId,
                versionNumber = version.VersionNumber,
                uploadedAt = version.UploadedAt
            });
        }

        [HttpGet("latest")]
        public IActionResult GetLatest(int docId)
        {
            var version = _versionService.GetLatestVersion(docId);
            if (version == null) return Ok(null);

            return Ok(new
            {
                versionId = version.VersionId,
                versionNumber = version.VersionNumber,
                originalText = version.OriginalText,
                uploadedAt = version.UploadedAt
            });
        }
    }
}
