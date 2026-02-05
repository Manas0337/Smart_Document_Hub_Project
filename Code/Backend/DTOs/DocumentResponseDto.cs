namespace Backend.DTOs
{
    public class DocumentResponseDto
    {
        public int DocId { get; set; }
        public string DocumentName { get; set; }
        public string DocumentDescription { get; set; }
        public string DocumentType { get; set; }

        public string FilePath { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
