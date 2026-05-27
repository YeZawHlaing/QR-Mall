using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ProductQrApi.Services;

public class PdfService
{
    public byte[] GenerateQrPdf(string qrImagePath, string productName)
    {
        byte[] pdfBytes;

        QuestPDF.Settings.License = LicenseType.Community;

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(50);

                page.Content().Column(col =>
                {
                    col.Item().Text("Product QR Code")
                        .FontSize(20)
                        .Bold();

                    col.Item().PaddingTop(10).Text(productName);

                    col.Item().PaddingTop(20).Image(qrImagePath);
                });
            });
        });

        pdfBytes = document.GeneratePdf();

        return pdfBytes;
    }
}