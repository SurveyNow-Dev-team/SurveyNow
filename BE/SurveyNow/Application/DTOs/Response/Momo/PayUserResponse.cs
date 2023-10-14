namespace Application.DTOs.Response.Momo
{
    public class PayUserResponse
    {
        public string partnerCode { get; set; } = string.Empty;
        public string orderId { get; set; } = string.Empty;
        public string requestId { get; set; } = string.Empty;
        public long amount { get; set; }
        public string transId { get; set; } = string.Empty;
        public long responseTime { get; set; }
        public int resultCode { get; set; }
        public string message { get; set; } = string.Empty;
        public long balance { get; set; }
    }
}
