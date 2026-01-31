using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers
{
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
        public IActionResult Upload(
            int docId,
            IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is required");

            int userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)
            );

            var version = _versionService.CreateVersion(
                docId, userId, file);

            return Ok(new
            {
                version.VersionId,
                version.VersionNumber,
                version.UploadedAt
            });
        }
    }
}
