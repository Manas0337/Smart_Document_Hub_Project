using DocumentFormat.OpenXml.Packaging;
using System.Text;
using UglyToad.PdfPig;

namespace Backend.Services
{
    public class DocumentTextExtractionService
    {
        
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

            private string ExtractPdf(IFormFile file)
            {
                var sb = new StringBuilder();

                using var stream = file.OpenReadStream();
                using var pdf = PdfDocument.Open(stream);

                foreach (var page in pdf.GetPages())
                {
                    sb.AppendLine(page.Text);
                }

                return Normalize(sb.ToString());
            }

            private string ExtractDocx(IFormFile file)
            {
                var sb = new StringBuilder();

                using var stream = file.OpenReadStream();
                using var doc = WordprocessingDocument.Open(stream, false);

                foreach (var para in doc.MainDocumentPart.Document.Body.Elements<DocumentFormat.OpenXml.Wordprocessing.Paragraph>())
                {
                    sb.AppendLine(para.InnerText);
                }

                return Normalize(sb.ToString());
            }

            private string ExtractTxt(IFormFile file)
            {
                using var reader = new StreamReader(file.OpenReadStream());
                return Normalize(reader.ReadToEnd());
            }

            private string Normalize(string text)
            {
                return text
                    .Replace("\r\n", "\n")
                    .Replace("\r", "\n")
                    .Trim();
            }
        }
    }
