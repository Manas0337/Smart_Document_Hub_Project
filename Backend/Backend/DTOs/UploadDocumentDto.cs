namespace Backend.DTOs
{
    public class UploadDocumentDto
    {
        public int DocId { get; set; }
        public IFormFile File { get; set; }
    }
}
