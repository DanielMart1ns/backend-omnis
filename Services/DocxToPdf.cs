// using System.Xml.Linq;
// using DocumentFormat.OpenXml.Packaging;
// using DocumentFormat.OpenXml.Wordprocessing;
// using OpenXmlPowerTools;
// using PdfSharpCore.Pdf;
// using PdfSharpCore.Pdf.IO;
// using QuestPDF.Fluent;
// using QuestPDF.Infrastructure;
// using VetCV.HtmlRendererCore.PdfSharpCore;
// using System.Drawing;
// using System.Drawing.Imaging;

// namespace DocxToPdf.Service;

// public class DocxToPdfService
// {
//     public string ConverterDocxEmHtml(Stream docxStream)
//     {
//         using var docx = WordprocessingDocument.Open(docxStream, true); // modo leitura

//         var settings = new HtmlConverterSettings
//         {
//             PageTitle = "Documento Word",
//             FabricateCssClasses = true,
//             RestrictToSupportedLanguages = false,
//             RestrictToSupportedNumberingFormats = false,

//             ImageHandler = imageInfo =>
//             {
//                 var extension = imageInfo.ContentType.Split('/')[1].ToLower();
//                 var imageFormat = extension switch
//                 {
//                     "png" => System.Drawing.Imaging.ImageFormat.Png,
//                     "jpeg" => System.Drawing.Imaging.ImageFormat.Jpeg,
//                     "gif" => System.Drawing.Imaging.ImageFormat.Gif,
//                     "bmp" => System.Drawing.Imaging.ImageFormat.Bmp,
//                     "tiff" => System.Drawing.Imaging.ImageFormat.Tiff,
//                     _ => null
//                 };

//                 if (imageFormat == null)
//                     return null;

//                 using var ms = new MemoryStream();
//                 imageInfo.Bitmap.Save(ms, imageFormat);
//                 var base64 = Convert.ToBase64String(ms.ToArray());
//                 var dataUri = $"data:{imageInfo.ContentType};base64,{base64}";

//                 var imgStyle = imageInfo.Bitmap.Width > 500
//                     ? "width: 500px; display: block; margin: auto; height:auto;"
//                     : "display: block; margin: auto; height:auto;";

//                 return new XElement(OpenXmlPowerTools.Xhtml.img,
//                     new XAttribute(NoNamespace.src, dataUri),
//                     new XAttribute("style", imgStyle),
//                     imageInfo.AltText != null ? new XAttribute(NoNamespace.alt, imageInfo.AltText) : null);
//             }
//         };

//         XElement htmlElement = HtmlConverter.ConvertToHtml(docx, settings);
//         var htmlContent = htmlElement.ToString();

//         File.WriteAllText("arquivo.html", htmlContent);
//         return htmlContent;
//     }

//     public (byte[] fullFileContent, byte[] previewFileContent) HtmlToPdf(string html, bool fileIsMoreThan1MB)
//     {
//         if(fileIsMoreThan1MB){

//             //add full file content
//             using var pdf = new PdfDocument();
//             PdfGenerator.AddPdfPages(pdf, html, PdfSharpCore.PageSize.A4);
            
//             using var fullStream = new MemoryStream();
//             pdf.Save(fullStream);
//             var fullFileContent = fullStream.ToArray();

//             //add preview file content
//             using var importStream = new MemoryStream(fullFileContent);
//             var fullPdf = PdfReader.Open(importStream, PdfDocumentOpenMode.Import);
//             var previewPdf = new PdfDocument();

//             for (int i = 0; i < Math.Min(3, fullPdf.PageCount); i++)
//             {
//                 previewPdf.AddPage(fullPdf.Pages[i]);
//             }

//             using var previewStream = new MemoryStream();
//             previewPdf.Save(previewStream);
//             var previewFileContent = previewStream.ToArray();

//             return (fullFileContent, previewFileContent);
//         }
//         else{
//             using var pdf = new PdfDocument();
//             PdfGenerator.AddPdfPages(pdf, html, PdfSharpCore.PageSize.A4);
            
//             using var memoryStream = new MemoryStream();
//             pdf.Save(memoryStream);
//             var fullFileContent = memoryStream.ToArray();
//             return (fullFileContent, Array.Empty<byte>());
//         }
//     }
// }