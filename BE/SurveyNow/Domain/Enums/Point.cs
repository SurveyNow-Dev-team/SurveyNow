namespace Domain.Enums
{
    public enum PaymentMethod
    {
        Momo = 1,
        VnPay = 2,
    }

    public enum PointSortingOrder
    {
        DateDescending = 1,
        DateAscending = 2,
        PointDescending = 3,
        PointAscending = 4,
    }

    public enum UserPointAction
    {
        DecreasePoint = 0,
        IncreasePoint = 1,
    }

    public enum Currency
    {
        VND,
        USD
    }

    public enum Platform
    {
        Mobile,
        Web
    }

    public enum TransactionHistorySortingOrder
    {
        DateDescending = 1,
        DateAscending = 2,
        AmountDescending = 3,
        AmountAscending = 4,
    }
    public enum TransactionHistoryType
    {

    }
}
