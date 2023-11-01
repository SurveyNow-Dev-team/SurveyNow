namespace Application.DTOs.Response.Transaction
{
    public class PointPurchaseTransactionCreateResponse
    {
        public required string Status { get; set; }
        public required string Message { get; set; }
        public decimal PointAmount { get; set; }
        public decimal MoneyAmount { get; set; }
        public required string Currency { get; set; }
        public required string PaymentMethod { get; set; }
        public required string DestinationAccount { get; set; }
        public required string Description { get; set; }
        public long TransactionId { get; set; }
    }
}
