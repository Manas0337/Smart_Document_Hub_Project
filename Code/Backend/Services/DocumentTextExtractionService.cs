using DocumentFormat.OpenXml.Packaging;
using System.Text;
using UglyToad.PdfPig;
using Microsoft.AspNetCore.Http;

namespace Backend.Services
{
    public class DocumentTextExtractionService
    {
        // ===============================
        // EXISTING METHOD (NO CHANGE)
        // ===============================
        public string ExtractText(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLower();

            return extension switch
            {
                ".pdf" => ExtractPdf(file),
                ".docx" => ExtractDocx(file),
                ".txt" => ExtractTxt(file),
                _ => throw new Exception("Unsupported file type")
            };
        }

        // ===============================
        // ✅ NEW METHOD (ADD THIS)
        // ===============================
        public string ExtractText(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLower();

            using var stream = File.OpenRead(filePath);

            return extension switch
            {
                ".pdf" => ExtractPdf(stream),
                ".docx" => ExtractDocx(stream),
                ".txt" => ExtractTxt(stream),
                _ => throw new Exception("Unsupported file type")
            };
        }

        // ===============================
        // PRIVATE HELPERS (STREAM-BASED)
        // ===============================
        private string ExtractPdf(Stream stream)
        {
            var sb = new StringBuilder();
            using var pdf = PdfDocument.Open(stream);

            foreach (var page in pdf.GetPages())
                sb.AppendLine(page.Text);

            return Normalize(sb.ToString());
        }

        private string ExtractDocx(Stream stream)
        {
            var sb = new StringBuilder();
            using var doc = WordprocessingDocument.Open(stream, false);

            foreach (var para in doc.MainDocumentPart.Document.Body
                .Elements<DocumentFormat.OpenXml.Wordprocessing.Paragraph>())
            {
                sb.AppendLine(para.InnerText);
            }

            return Normalize(sb.ToString());
        }

        private string ExtractTxt(Stream stream)
        {
            using var reader = new StreamReader(stream);
            return Normalize(reader.ReadToEnd());
        }

        // ===============================
        // ORIGINAL HELPERS (IFormFile)
        // ===============================
        private string ExtractPdf(IFormFile file)
            => ExtractPdf(file.OpenReadStream());

        private string ExtractDocx(IFormFile file)
            => ExtractDocx(file.OpenReadStream());

        private string ExtractTxt(IFormFile file)
            => ExtractTxt(file.OpenReadStream());

        private string Normalize(string text)
        {
            return text
                .Replace("\r\n", "\n")
                .Replace("\r", "\n")
                .Trim();
        }
    }
}
