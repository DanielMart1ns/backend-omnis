using Microsoft.AspNetCore.Mvc;
using System.Text.Json; 
using PdfGenerator.Service;
// using DocxToPdf.Service;

namespace Pdf.Controller;

[ApiController]
[Route("api/pdf")]
public class PdfController : ControllerBase
{
    private readonly PdfGeneratorService _pdfService;

    public PdfController(PdfGeneratorService pdfService)
    {
        _pdfService = pdfService;
    }

    [HttpPost("generate")]
    public async Task<IActionResult> ConvertToPdf(JsonElement content)
    {
        var pdfBytes = _pdfService.GeneratePdfFromJson(content);
        var base64Pdf = Convert.ToBase64String(pdfBytes);
        return Ok(base64Pdf);
    }
}

// [ApiController]
// [Route("api/pdf")]
// public class PdfController : ControllerBase
// {
//     private readonly DocxToPdfService _pdfService;

//     public PdfController(DocxToPdfService pdfService)
//     {
//         _pdfService = pdfService;
//     }

//     [HttpPost("from-docx")]
//     public async Task<IActionResult> ConverterDocxEmHtml(IFormFile file, bool fileIsMoreThan1MB)
//     {
//         using var stream = file.OpenReadStream();
//         using var writableStream = new MemoryStream();
//         await stream.CopyToAsync(writableStream);
//         writableStream.Position = 0;
//         var html = _pdfService.ConverterDocxEmHtml(writableStream);

//         var pdfBytes = _pdfService.HtmlToPdf(html, fileIsMoreThan1MB);
//         var (fullFileContent, previewFileContent) = pdfBytes;

//         if(fileIsMoreThan1MB)
//         {            
//             return Ok(new 
//             {
//                 fullFile = Convert.ToBase64String(fullFileContent),
//                 previewFile = Convert.ToBase64String(previewFileContent)
//             });
//         }

//         return File(fullFileContent, "application/pdf", "arquivo.pdf");
//     }
// }