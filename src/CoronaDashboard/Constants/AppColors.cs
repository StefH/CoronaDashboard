﻿using Blazorise.Charts;

namespace CoronaDashboard.Constants
{
    public static class AppColors
    {
        public static string Black = "#000000";

        public static string Gray = "#808080";

        public static string LightGray = "#D3D3D3";

        public static string Red = "#DC4040";

        public static string Blue = "#9BC2E6";

        public static string Green = "#A9D08E";

        public static string Yellow = "#FFD966";

        public static string DarkBlue = "#3296C8";

        public static ChartColor ChartBlack;

        public static ChartColor ChartDarkBlue;

        public static ChartColor ChartGray;

        public static ChartColor ChartLightGray;

        public static ChartColor ChartRed;

        public static ChartColor ChartBlue;

        public static ChartColor ChartGreen;

        public static ChartColor ChartYellow;

        static AppColors()
        {
            ChartBlack = ChartColor.FromHtmlColorCode(Black);

            ChartDarkBlue = ChartColor.FromHtmlColorCode(DarkBlue);

            ChartLightGray = ChartColor.FromHtmlColorCode(LightGray);

            ChartGray = ChartColor.FromHtmlColorCode(Gray);

            ChartRed = ChartColor.FromHtmlColorCode(Red);

            ChartBlue = ChartColor.FromHtmlColorCode(Blue);

            ChartGreen = ChartColor.FromHtmlColorCode(Green);

            ChartYellow = ChartColor.FromHtmlColorCode(Yellow);
        }
    }
}