using System.Globalization;
using System.Text;

namespace Karatombola;

public static class SecondsToStringConverter
{
    public static object Convert(object value)
    {
        if (value is not double doubleValue)
        {
            return value;
        }

        StringBuilder formatBuilder = new();
        var timeSpan = TimeSpan.FromSeconds(doubleValue);

        if (timeSpan.Hours > 0)
        {
            formatBuilder.Append(@"hh\:");
        }

        formatBuilder.Append(@"mm\:ss");

        return timeSpan.ToString(formatBuilder.ToString());
    }
}