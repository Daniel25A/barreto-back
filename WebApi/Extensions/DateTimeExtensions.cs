using System.Globalization;

namespace WebApi.Extensions;

public static class DateTimeExtensions
{
    public static string GetMonthName(this DateTime dateTime)
    {
        return dateTime.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")).ToUpper();
    }
}