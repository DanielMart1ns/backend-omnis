using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Text.Json; 

namespace PdfGenerator.Service;

// code in your main method
public class PdfGeneratorService
{
    public byte[] GeneratePdfFromJson(JsonElement content)
    {
        var pdfBytes = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header()
                    .Row(row =>
                        {
                            row.ConstantItem(60)
                                .Height(60)
                                .AlignMiddle()
                                .Image("assets/logo.png");

                            row.RelativeItem()
                                .AlignMiddle()
                                .PaddingLeft(10)
                                .Text("Nucleu Tecnologia")
                                .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);
                        });

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(col =>
                    {
                        col.Spacing(10);

                        foreach (var property in content.EnumerateObject())
                        {
                            col.Item().Text($"{property.Name}: {property.Value.GetString()}");
                        }
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                        x.Span(" of ");
                        x.TotalPages();
                    });
            });
        }).GeneratePdf();

        return pdfBytes;
    }
}