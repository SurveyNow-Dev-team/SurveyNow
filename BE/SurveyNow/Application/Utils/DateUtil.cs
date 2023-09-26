using System.Globalization;

namespace Application.Utils;

public static class DateUtil
{
    public static string FormatDateTimeToDateV1(DateTime? date) => date?.ToString("dd/MM/yyyy") ?? "";
    public static string FormatDateTimeToDateV2(DateTime? date) => date?.ToString("dd/MM/yyyy dddd") ?? "";
    public static string FormatDateTimeToDatetimeV1(DateTime? date) => date?.ToString("dd/MM/yyyy HH:mm:ss") ?? "";
    public static string FormatDateTimeToDatetimeV2(DateTime? date) => date?.ToString("dd/MM/yyyy HH:mm:ss dddd") ?? "";

    public static DateTime? ConvertStringToDateTimeV1(string? dateString)
    {
        if (string.IsNullOrEmpty(dateString))
        {
            return null;
        }

        DateTime date;
        if (DateTime.TryParseExact(dateString, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None,
                out date))
        {
            return date;
        }
        else
        {
            return null;
        }
    }

    public static DateTime? ConvertStringToDateTimeV2(string? dateString)
    {
        if (string.IsNullOrEmpty(dateString))
        {
            return null;
        }

        DateTime date;
        if (DateTime.TryParseExact(dateString, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None,
                out date))
        {
            return date;
        }
        else
        {
            return null;
        }
    }
}