using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Blazorise.Charts;

namespace CoronaDashboard
{
    public static class ChartColorExtensions
    {
        private static readonly Regex HtmlColorRegex = new Regex(@"^#((?'R'[0-9a-f]{2})(?'G'[0-9a-f]{2})(?'B'[0-9a-f]{2}))|((?'R'[0-9a-f])(?'G'[0-9a-f])(?'B'[0-9a-f]))$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Based on https://stackoverflow.com/questions/982028/convert-net-color-objects-to-hex-codes-and-back
        /// </summary>
        public static ChartColor FromHex(string hexString)
        {
            if (hexString == null)
            {
                throw new ArgumentNullException(nameof(hexString));
            }

            var match = HtmlColorRegex.Match(hexString);
            if (!match.Success)
            {
                throw new ArgumentException($"The string \"{hexString}\" doesn't represent a valid HTML hexadecimal color.", nameof(hexString));
            }

            return new ChartColor(ParseHexValueAsByte(match.Groups["R"].Value), ParseHexValueAsByte(match.Groups["G"].Value), ParseHexValueAsByte(match.Groups["B"].Value));
        }

        private static byte ParseHexValueAsByte(string value)
        {
            return byte.Parse(value, NumberStyles.AllowHexSpecifier);
        }
    }
}