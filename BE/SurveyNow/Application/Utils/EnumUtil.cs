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

        public static string ConvertSurveyStatusToString(SurveyStatus surveyStatus)
        {
            switch (surveyStatus)
            {
                case SurveyStatus.Active:
                    return "Active";
                case SurveyStatus.InActive:
                    return "Inactive";
                case SurveyStatus.Draft:
                    return "Draft";
                case SurveyStatus.Expired:
                    return "Expired";
                default:
                    return "";
            }
        }

        public static string ConvertTransactionStatusToString(TransactionStatus transactionStatus)
        {
            switch (transactionStatus)
            {
                case TransactionStatus.Fail:
                    return "Failed";
                case TransactionStatus.Success:
                    return "Success";
                default:
                    return "";
            }
        }

    }
}
