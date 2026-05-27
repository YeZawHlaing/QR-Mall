using QRCoder;

namespace ProductQrApi.Helpers;

public static class QrCodeGeneratorHelper
{
    public static string GenerateQrCode(string content, string filePath)
    {
        using QRCodeGenerator generator = new QRCodeGenerator();

        using QRCodeData data =
            generator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);

        PngByteQRCode qrCode = new PngByteQRCode(data);

        byte[] qrBytes = qrCode.GetGraphic(20);

        File.WriteAllBytes(filePath, qrBytes);

        return filePath;
    }

}