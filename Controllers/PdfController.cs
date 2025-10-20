using Microsoft.AspNetCore.Mvc;

using PdfGenerator.Service;
using EmailSender.Service;
using WppSender.Service;

using System.Text.Json;

namespace Pdf.Controller;

[ApiController]
[Route("api/pdf")]
public class PdfController : ControllerBase
{
    private readonly PdfGeneratorService _pdfService;
    private readonly EmailSenderService _emailService;
    private readonly WppSenderService _wppService;

    public PdfController(PdfGeneratorService pdfService,
                            EmailSenderService emailService,
                            WppSenderService wppService)
    {
        _pdfService = pdfService;
        _emailService = emailService;
        _wppService = wppService;
    }

    [HttpPost("generate")]
    public async Task<IActionResult> ConvertToPdf(JsonElement content)
    {
        byte[] pdfBytes = _pdfService.GeneratePdfFromJson(content);
        string base64Pdf = Convert.ToBase64String(pdfBytes);

        //Mandar email automatico
        // bool emailSended = await _emailService.SendEmail(base64Pdf);

        // bool fileSendedToWpp = await _wppService.SendFileToWpp(pdfBytes);

        return Ok(base64Pdf);
    }
}