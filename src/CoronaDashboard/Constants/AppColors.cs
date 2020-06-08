using Blazorise.Charts;

namespace CoronaDashboard.Constants
{
    public static class AppColors
    {
        public static string LightGray = "#D3D3D3";

        public static string Red = "#DCA0A0";

        public static string Blue = "#9BC2E6";

        public static string Green = "#A9D08E";

        public static string Yellow = "#FFD966";

        public static ChartColor ChartDarkBlue = ChartColor.FromRgba(50, 150, 200, 1f);

        public static ChartColor ChartLightGray = ChartColorExtensions.FromHex(LightGray);

        public static ChartColor ChartRed = ChartColorExtensions.FromHex(Red);

        public static ChartColor ChartBlue = ChartColorExtensions.FromHex(Blue);

        public static ChartColor ChartGreen = ChartColorExtensions.FromHex(Green);

        public static ChartColor ChartYellow = ChartColorExtensions.FromHex(Yellow);
    }
}