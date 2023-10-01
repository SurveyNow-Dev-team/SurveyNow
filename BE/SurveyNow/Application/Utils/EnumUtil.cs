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

        public static string GeneratePointHistoryDescription(PointHistoryType type, long userId, decimal point,
            long surveyId = 0, PackType packType = PackType.Basic, PaymentMethod paymentMethod = PaymentMethod.Momo)
        {
            switch (type)
            {
                case PointHistoryType.PurchasePoint:
                    return $"User purchase point to their account. " +
                           $"User ID: {userId}; " +
                           $"Point Amount: {point}; " +
                           $"Total Amount: {point * BusinessData.BasePointVNDPrice} VND; " +
                           $"With method: {paymentMethod.ToString()}; ";

                case PointHistoryType.DoSurvey:
                    return $"User received point from survey completion. " +
                           $"User ID: {userId}; " +
                           $"Point Amount: {point}; " +
                           $"Survey ID: {surveyId}; ";

                case PointHistoryType.GiftPoint:
                    return "";

                case PointHistoryType.RefundPoint:
                    return $"User received refunded point. " +
                           $"User ID: {userId}; " +
                           $"Point Amount: {point}; " +
                           $"Reason: ; ";

                case PointHistoryType.RedeemPoint:
                    return $"User redeem point. " +
                           $"User ID: {userId}; " +
                           $"Point Amount: {point}; ";

                case PointHistoryType.PackPurchase:
                    return $"User purchase pack for posting survey. " +
                           $"User ID: {userId}; " +
                           $"Pack type: {Enum.GetName(packType)}; " +
                           $"Survey ID: {surveyId}; " +
                           $"Point Amount: {point}";

                case PointHistoryType.ReceiveGift:
                    return "";

                default:
                    return "";
            }
        }

        public static T? ConvertStringToEnum<T>(string? input) where T : struct, Enum
        {
            if (string.IsNullOrEmpty(input))
                return null;

            if (Enum.TryParse<T>(input, out T result))
                return result;

            return null;
        }
    }
}