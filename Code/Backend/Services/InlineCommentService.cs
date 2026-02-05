using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class InlineCommentService
    {
        private readonly AppDbContext _context;

        public InlineCommentService(AppDbContext context)
        {
            _context = context;
        }

        public InlineComment AddComment(CreateInlineCommentDto dto, int userId)
        {
            var version = _context.DocumentVersions
                .FirstOrDefault(v => v.VersionId == dto.VersionId);

            if (version == null)
                throw new Exception("Document version not found");

            if (dto.StartIndex < 0 || dto.EndIndex > version.OriginalText.Length)
                throw new Exception("Invalid text range");

            var comment = new InlineComment
            {
                VersionId = dto.VersionId,
                StartIndex = dto.StartIndex,
                EndIndex = dto.EndIndex,
                CommentText = dto.CommentText,
                AuthorId = userId
            };

            _context.InlineComments.Add(comment);
            _context.SaveChanges();

            return comment;
        }

        public List<InlineComment> GetCommentsByVersion(int versionId)
        {
            return _context.InlineComments
                .Where(c => c.VersionId == versionId)
                .Include(c => c.Author)
                .OrderBy(c => c.StartIndex)
                .ToList();
        }

        public void UpdateComment(int commentId, string newText, int userId)
        {
            var comment = _context.InlineComments.Find(commentId);

            if (comment == null)
                throw new Exception("Comment not found");

            if (comment.AuthorId != userId)
                throw new UnauthorizedAccessException();

            comment.CommentText = newText;
            _context.SaveChanges();
        }

        public void DeleteComment(int commentId, int userId)
        {
            var comment = _context.InlineComments.Find(commentId);

            if (comment == null)
                throw new Exception("Comment not found");

            if (comment.AuthorId != userId)
                throw new UnauthorizedAccessException();

            _context.InlineComments.Remove(comment);
            _context.SaveChanges();
        }
    }
}
