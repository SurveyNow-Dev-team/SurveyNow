using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request.Point
{
    public class PointRedeemRequest
    {
        public decimal PointAmount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public long UserId { get; set; }
        [RegularExpression(@"(84|0[3|5|7|8|9])+([0-9]{8})\b", ErrorMessage = "Invalid phone number")]
        [MinLength(10, ErrorMessage = "Invalid phone number")]
        public required string MomoAccount { get; set; }
    }
}
