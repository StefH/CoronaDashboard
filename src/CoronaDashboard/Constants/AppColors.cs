using Blazorise.Charts;

namespace CoronaDashboard.Constants
{
    public static class AppColors
    {
        public static string LightGray = "#D3D3D3";

        public static string Red = "#DC0000";

        public static string Blue = "#9BC2E6";

        public static string Green = "#A9D08E";

        public static string Yellow = "#FFD966";

        public static string DarkBlue = "#3296C8";

        public static ChartColor ChartDarkBlue = ChartColorExtensions.FromHex(DarkBlue);

        public static ChartColor ChartLightGray = ChartColorExtensions.FromHex(LightGray);

        public static ChartColor ChartRed = ChartColorExtensions.FromHex(Red);

        public static ChartColor ChartBlue = ChartColorExtensions.FromHex(Blue);

        public static ChartColor ChartGreen = ChartColorExtensions.FromHex(Green);

        public static ChartColor ChartYellow = ChartColorExtensions.FromHex(Yellow);
    }
}