namespace Application.DTOs.Response.Point
{
    public class ShortPointPurchaseResponse
    {
        public long Id { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } = null!;
    }
}
