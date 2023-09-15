namespace Application.Utils;

public static class DateUtil
{
    public static string FormatDateTimeToDateV1(DateTime date) => date.ToString("dd/MM/yyyy");
    public static string FormatDateTimeToDateV2(DateTime date) => date.ToString("dd/MM/yyyy dddd");
    public static string FormatDateTimeToDatetimeV1(DateTime date) => date.ToString("dd/MM/yyyy HH:mm:ss");
    public static string FormatDateTimeToDatetimeV2(DateTime date) => date.ToString("dd/MM/yyyy HH:mm:ss dddd");
}