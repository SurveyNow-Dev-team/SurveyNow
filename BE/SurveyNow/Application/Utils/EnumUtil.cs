using Domain.Enums;

namespace Application.Utils
{
    public static class EnumUtil
    {
        public static string ConvertPointHistoryTypeToString(PointHistoryType pointHistoryType)
        {
            switch (pointHistoryType)
            {
                case PointHistoryType.PurchasePoint:
                    return "Purchase Point";
                case PointHistoryType.DoSurvey:
                    return "Complete Survey";
                case PointHistoryType.GiftPoint:
                    return "Gifting Point";
                case PointHistoryType.RefundPoint:
                    return "Refund Point";
                case PointHistoryType.RedeemPoint:
                    return "Reddem Gift";
                case PointHistoryType.PackPurchase:
                    return "Purchase Pack";
                case PointHistoryType.ReceiveGift:
                    return "Receive Gifted Point";
                default:
                    return "";
            }
        }

        public static string ConvertTransactionTypeToString(TransactionType transactionType)
        {
            switch (transactionType)
            {
                case TransactionType.PurchasePoint:
                    return "Purchase Point";
                case TransactionType.RedeemGift:
                    return "Reddem Gift";
                case TransactionType.RefundMoney:
                    return "Refund Money";
                default:
                    return "";
            }
        }
    }
}
