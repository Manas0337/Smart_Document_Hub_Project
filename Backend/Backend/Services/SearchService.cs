using Backend.Data;
using Backend.DTOs;
using Backend.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class SearchService
    {
        private readonly AppDbContext _context;

        public SearchService(AppDbContext context)
        {
            _context = context;
        }

        public List<SearchResultDto> Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                throw new BadRequestException("Search keyword cannot be empty");

            keyword = keyword.ToLower();

            var results = _context.DocumentVersions
                .Include(v => v.Document)
                .Where(v => v.OriginalText.ToLower().Contains(keyword))
                .Select(v => new SearchResultDto
                {
                    DocumentId = v.DocId,
                    Title = v.Document.DocumentName,
                    VersionId = v.VersionId,
                    VersionNumber = v.VersionNumber,
                    Snippet = GetSnippet(v.OriginalText, keyword)
                })
                .Take(20)
                .ToList();

            return results;
        }

        private static string GetSnippet(string text, string keyword)
        {
            var index = text.ToLower().IndexOf(keyword);

            if (index == -1)
                return string.Empty;

            int start = Math.Max(0, index - 30);
            int length = Math.Min(100, text.Length - start);

            return text.Substring(start, length) + "...";
        }
    }
}
