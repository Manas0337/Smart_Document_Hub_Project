using Backend.DTOs;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    public class InlineCommentsController : ControllerBase
    {
        private readonly InlineCommentService _service;

        public InlineCommentsController(InlineCommentService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult Create(CreateInlineCommentDto dto)
        {
            int userId = int.Parse(User.FindFirst("UserId").Value);
            var comment = _service.AddComment(dto, userId);
            return Ok(comment);
        }

        [HttpGet("version/{versionId}")]
        public IActionResult GetByVersion(int versionId)
        {
            return Ok(_service.GetCommentsByVersion(versionId));
        }

        [HttpPut("{commentId}")]
        public IActionResult Update(int commentId, UpdateInlineCommentDto dto)
        {
            int userId = int.Parse(User.FindFirst("UserId").Value);
            _service.UpdateComment(commentId, dto.CommentText, userId);
            return Ok();
        }

        [HttpDelete("{commentId}")]
        public IActionResult Delete(int commentId)
        {
            int userId = int.Parse(User.FindFirst("UserId").Value);
            _service.DeleteComment(commentId, userId);
            return Ok();
        }
    }
}
