namespace Application.DTOs.Request.Transaction
{
    public class UpdatePointPurchaseTransactionRequest
    {
        public required string EWalletTransactionId { get; set; }
        public required string SourceAccount { get; set; }
    }
}
