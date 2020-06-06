using System;
using System.Globalization;

namespace CoronaDashboard.Utils
{
    public static class DateUtils
    {
        private static readonly CultureInfo CultureNederland = new CultureInfo("NL-nl");

        private const string DateFormat = "dd-MMM";
        private const string LongDateFormat = "d MMM yyyy";

        public static string ToShortDate(DateTime date)
        {
            return date.ToString(DateFormat, CultureNederland);
        }

        public static string ToLongDate(DateTime date)
        {
            return date.ToString(LongDateFormat, CultureNederland);
        }
    }
}