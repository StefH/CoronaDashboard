using System;
using System.Globalization;

namespace CoronaDashboard.Utils
{
    public static class DateUtils
    {
        private const string Today = "vandaag";
        private static readonly CultureInfo CultureNederland = CultureInfo.GetCultureInfo("nl-NL");

        public static string ToTodayOrDayWithWithLongMonth(DateTime date)
        {
            return date.Date == DateTime.Today ? Today : string.Format(CultureNederland, "{0:dd MMMM}", date);
        }

        public static string ToDayWithLongMonth(DateTime date)
        {
            return string.Format(CultureNederland, "{0:dd MMMM}", date);
        }

        public static string ToDayWithShortMonth(DateTime date)
        {
            return string.Format(CultureNederland, "{0:dd-MMM}", date);
        }

        public static string ToDayWithShortMonthAndYear(DateTime date)
        {
            return string.Format(CultureNederland, "{0:d MMM yyyy}", date);
        }
    }
}