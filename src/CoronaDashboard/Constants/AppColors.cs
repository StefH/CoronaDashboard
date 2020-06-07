using Blazorise.Charts;

namespace CoronaDashboard.Constants
{
    public static class AppColors
    {
        public static string Red = "#DCA0A0";

        public static string Blue = "#9BC2E6";

        public static string Green = "#A9D08E";

        public static string Yellow = "#FFD966";

        public static ChartColor LineChartBlue = ChartColor.FromRgba(50, 150, 200, 1f);

        public static ChartColor BarChartRed = ChartColorExtensions.FromHex(Red);

        public static ChartColor BarChartBlue = ChartColorExtensions.FromHex(Blue);

        public static ChartColor BarChartGreen = ChartColorExtensions.FromHex(Green);

        public static ChartColor BarChartYellow = ChartColorExtensions.FromHex(Yellow);
    }
}