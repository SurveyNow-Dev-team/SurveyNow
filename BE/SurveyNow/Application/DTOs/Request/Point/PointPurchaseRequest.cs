﻿using Domain.Enums;

namespace Application.DTOs.Request.Point
{
    public class PointPurchaseRequest
    {
        public decimal PointAmount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public long UserId { get; set; }
        public Platform Platform { get; set; }
    }
}
