using System;
using System.Globalization;

namespace CoronaDashboard.Utils
{
    public static class DateUtils
    {
        private static readonly CultureInfo CultureNederland = new CultureInfo("NL-nl");

        private const string DateFormat = "dd-MM-yyyy";
        private const string LongDateFormat = "d MMMM yyyy";

        public static string ToLongDate(DateTime date)
        {
            return date.ToString(LongDateFormat, CultureNederland);
        }
    }
}