using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application.DTOs.Response.Transaction
{
    public class TransactionResponse
    {
        public long Id { get; set; }

        public required string TransactionType { get; set; }

        public required string PaymentMethod { get; set; }

        [Precision(10, 1)]
        public decimal Point { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; } = null!;

        public required string Date { get; set; }

        public string SourceAccount { get; set; } = null!;

        public string DestinationAccount { get; set; } = null!;

        public string PurchaseCode { get; set; } = null!;

        public required string Status { get; set; }

        public long UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
    }
}
