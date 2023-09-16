using Microsoft.EntityFrameworkCore;

namespace Application.DTOs.Response.Pack
{
    public class PackPurchaseResponse
    {
        public long Id { get; set; }

        public required string Date { get; set; }

        public required string PackType { get; set; }

        [Precision(10, 1)]
        public decimal Point { get; set; }

        public required string Status { get; set; }

        public long UserId { get; set; }
    }
}
