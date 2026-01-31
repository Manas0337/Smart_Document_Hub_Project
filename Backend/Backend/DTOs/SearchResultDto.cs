namespace Backend.DTOs
{
    public class SearchResultDto
    {
        public int DocumentId { get; set; }
        public string Title { get; set; }

        public int VersionId { get; set; }
        public int VersionNumber { get; set; }

        //shows small text preview
        public string Snippet { get; set; }
    }
}
