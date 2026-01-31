namespace Backend.DTOs
{
    public class CreateInlineCommentDto
    {
        public int VersionId { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public string CommentText { get; set; }
    }
}
