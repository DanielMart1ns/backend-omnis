using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;

namespace EmailSender.Service;

public class EmailSenderService
{
    private readonly MailjetClient _client;

    public EmailSenderService(IConfiguration config)
    {
        _client = new MailjetClient(
            config["Mailjet:ApiKey"],
            config["Mailjet:SecretKey"]
        );
    }

    public async Task<bool> SendEmail(string pdfContent)
    {
        var request = new MailjetRequest
        {
            Resource = Send.Resource,
        }
        .Property(Send.FromEmail, "ebeesystem@gmail.com")
        .Property(Send.FromName, "Nucleu Omnis")
        .Property(Send.Subject, "Seu PDF está pronto")
        .Property(Send.TextPart, "Segue o PDF em anexo.")
        .Property(Send.HtmlPart, "<h3>Segue o PDF gerado com seus dados.</h3>")
        .Property(Send.Recipients, new JArray {
            new JObject {
                { "Email", "danielmmrodrigues09@gmail.com" }
            }
        })
        .Property(Send.Attachments, new JArray {
            new JObject {
                { "Content-type", "application/pdf" },
                { "Filename", "arquivo.pdf" },
                { "content", pdfContent }
            }
        });

        var response = await _client.PostAsync(request);

        if (response.IsSuccessStatusCode)
        {
            return true;
        }

        return false;

        // bool emailSended = response.Messages != null && response.Messages.Length == 1;

        // return emailSended;
    }
}