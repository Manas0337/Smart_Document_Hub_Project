namespace Backend.DTOs
{
    public class CreateDocumentDto
    {
        public string DocumentName { get; set; }
        public string? DocumentDescription { get; set; }
        public string DocumentType { get; set; }

        //public IFormFile File { get; set; }   
    }
}
