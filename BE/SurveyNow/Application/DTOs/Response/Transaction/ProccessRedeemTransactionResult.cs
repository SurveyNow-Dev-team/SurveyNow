namespace Application.DTOs.Response.Transaction
{
    public class ProccessRedeemTransactionResult
    {
        public required string Status { get; set; }
        public required string Message { get; set; }
        public long TransactionId { get; set; }
    }
}
