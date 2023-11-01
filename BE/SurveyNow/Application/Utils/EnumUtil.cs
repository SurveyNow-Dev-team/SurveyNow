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
                case TransactionStatus.Pending:
                    return "Pending";
                case TransactionStatus.Cancel:
                    return "Cancel";
                default:
                    return "";
            }
        }

        public static string GeneratePointHistoryDescription(PointHistoryType type, long userId, decimal point,
            long surveyId = 0, PackType packType = PackType.Basic, PaymentMethod paymentMethod = PaymentMethod.Momo, string refundReason = "")
        {
            switch (type)
            {
                case PointHistoryType.PurchasePoint:
                    return $"Người dùng nạp điểm vào tài khoản - " +
                           $"Số lượng điểm: {point} - " +
                           $"Số lượng tiền: {point * BusinessData.BasePointVNDPrice} VND - " +
                           $"Phương thức thanh toán: {paymentMethod.ToString()}";

                case PointHistoryType.DoSurvey:
                    return $"Người dùng nhận thưởng điểm sau khi đã hoàn thành khảo sát - " +
                           $"Số lượng điểm: {point} - " +
                           $"Mã số khảo sát: {surveyId}";

                case PointHistoryType.GiftPoint:
                    return "";

                case PointHistoryType.RefundPoint:
                    return $"Người dùng được hoàn lại điểm - " +
                           $"Số lượng điểm: {point} - " +
                           $"Lý do: {refundReason}";

                case PointHistoryType.RedeemPoint:
                    return $"Người dùng đổi điểm - " +
                           $"Số lượng điểm: {point} - " +
                           $"Số lượng tiền: {point * BusinessData.BasePointVNDPrice} VND - " +
                           $"Phương thức thanh toán: {paymentMethod.ToString()}";

                case PointHistoryType.PackPurchase:
                    return $"Người dùng mua gói dùng để đăng khảo sát - " +
                           $"Loại gói: {GetPackTypeName(packType)} - " +
                           $"Mã số khảo sát: {surveyId} - " +
                           $"Số lượng điểm: {point}";

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

        public static string GetPackTypeName(PackType packType)
        {
            switch (packType)
            {
                case PackType.Basic:
                    return "Gói Tiết Kiệm";
                case PackType.Medium:
                    return "Gói Cơ Bản";
                case PackType.Advanced:
                    return "Gói Nâng Cao";
                case PackType.Expert:
                    return "Gói Chuyên Gia";
                default:
                    return string.Empty;
            }
        }
    }
}