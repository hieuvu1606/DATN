using QRCoder;

namespace DATN.Utils
{
    public static class QRGenerator
    {
        public static string QRconvert(int id)
        {
            string parameters = id.ToString();

            string fullUrl = $"{parameters}";

            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(fullUrl, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeBytes = qrCode.GetGraphic(25);

            string base64String = Convert.ToBase64String(qrCodeBytes);

            return base64String;
        }
    }
}
