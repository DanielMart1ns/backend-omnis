using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WppSender.Service;

public class WppSenderService
{
    public async Task<bool> SendFileToWpp(byte[] pdfBytes)
    {
        string token = "Bearer EAASFLZAamZALQBPm9b2KqUvfFvBG9MHeK80d1OTDKXhtYlt2Ch76Tti8uxZBhjhFZCGiShw86QYdxBLPApVnSYTLNhq9sBPdfrZBWtisMzkP6USdMUcjj02jG2yriOWJLlt6Kn7dchHQxUw0seuxva0SrrSJPeAlZBvsn8DQkI3Ao7MXjQUaSIAZCLbtBlyRfcqZCBs9gthbkd5azABWW6Rl9HlvDmaIxeX3oXz0Y6Pf6gZDZD";

        string mediaId = await UploadFileToMetaServer(pdfBytes, token);

        var client = new HttpClient();

        client.DefaultRequestHeaders.Add("Authorization", token);

        var payload = new
        {
            messaging_product = "whatsapp",
            to = "5511940334631",
            type = "document",
            document = new
            {
                id = mediaId,
                caption = "Nucleu Omnis PDF",
                filename = "arquivo.pdf"
            }
        };

        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync("https://graph.facebook.com/v23.0/783343481517493/messages", content);

        if (response.IsSuccessStatusCode)
        {
            return true;
        }

        return false;
    }

    private async Task<string> UploadFileToMetaServer(byte[] pdfBytes, string token)
    {
        var client = new HttpClient();

        client.DefaultRequestHeaders.Add("Authorization", token);

        var form = new MultipartFormDataContent();

        var byteContent = new ByteArrayContent(pdfBytes);
        byteContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/pdf");

        form.Add(byteContent, "file", "arquivo.pdf");
        form.Add(new StringContent("whatsapp"), "messaging_product");

        var response = await client.PostAsync("https://graph.facebook.com/v23.0/783343481517493/media", form);

        var responseContent = await response.Content.ReadAsStringAsync();

        Console.WriteLine("Resposta da Meta:");
        Console.WriteLine(responseContent);

        var mediaId = JsonDocument.Parse(responseContent).RootElement.GetProperty("id").GetString();
        return mediaId;
    }
}