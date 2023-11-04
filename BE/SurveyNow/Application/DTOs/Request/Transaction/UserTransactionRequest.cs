using Domain.Enums;

namespace Application.DTOs.Request.Transaction
{
    public class UserTransactionRequest
    {
        public TransactionType? TransactionType { get; set; }
        public TransactionStatus? Status { get; set; }
    }
}
