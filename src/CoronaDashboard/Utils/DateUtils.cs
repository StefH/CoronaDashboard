using System;
using System.Globalization;

namespace CoronaDashboard.Utils
{
    public static class DateUtils
    {
        private static readonly CultureInfo CultureNederland = CultureInfo.GetCultureInfo("nl-NL");

        public static string ToShortDate(DateTime date)
        {
            return string.Format(CultureNederland, "{0:dd-MMM}", date);
        }

        public static string ToLongDate(DateTime date)
        {
            return string.Format(CultureNederland, "{0:d MMM yyyy}", date);
        }
    }
}