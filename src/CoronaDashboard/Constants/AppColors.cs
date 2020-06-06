using Blazorise.Charts;

namespace CoronaDashboard.Constants
{
    public static class AppColors
    {
        public static string Red = "#DCA0A0";

        public static string Blue = "#9CC2E8";

        public static string Green = "#B3CFB9";

        public static string Yellow = "#E7C2A0";

        public static ChartColor LineChartBlue = ChartColor.FromRgba(50, 150, 200, 1f);

        public static ChartColor BarChartRed = ChartColorExtensions.FromHex(Red);

        public static ChartColor BarChartBlue = ChartColorExtensions.FromHex(Blue);

        public static ChartColor BarChartGreen = ChartColorExtensions.FromHex(Green);

        public static ChartColor BarChartYellow = ChartColorExtensions.FromHex(Yellow);
    }
}