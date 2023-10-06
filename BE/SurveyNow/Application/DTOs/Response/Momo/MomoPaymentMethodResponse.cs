namespace Application.DTOs.Response.Momo
{
    public class MomoPaymentMethodResponse
    {
        public string payUrl { get; set; } = string.Empty;
        public string qrCodeUrl { get; set; } = string.Empty;
        public string deeplink { get; set; } = string.Empty;
    }
}
