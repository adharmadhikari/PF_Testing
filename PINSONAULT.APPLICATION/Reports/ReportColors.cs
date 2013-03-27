using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Pinsonault.Data.Reports
{
    /// <summary>
    /// Summary description for ReportColors
    /// </summary>
    public static class ReportColors
    {
        static ReportColors()
        {
            CustomerContactReports = new ColorPalette(new Color[] { Color.SteelBlue, Color.Wheat, Color.PowderBlue, Color.Gray, Color.Peru, Color.Navy, Color.Maroon, Color.LightSteelBlue, Color.DarkGreen, Color.Coral, Color.Yellow, Color.DarkBlue, Color.Green, Color.Red, Color.BurlyWood, Color.Tomato });
            StandardReports = new ColorPalette(new Color[] { Color.SteelBlue, Color.Wheat, Color.PowderBlue, Color.Gray, Color.Peru, Color.Navy, Color.Maroon, Color.PaleGreen, Color.DarkGreen, Color.Coral, Color.Yellow, Color.DarkBlue, Color.Green, Color.Red, Color.BurlyWood, Color.Tomato });
            StandardReports101 = new ColorPalette(new Color[] { Color.PaleGreen, Color.Coral, Color.Maroon, Color.LightSteelBlue, Color.Yellow, Color.PowderBlue, Color.Gray, Color.Peru, Color.Maroon, Color.DarkGreen, Color.Green, Color.Red, Color.BurlyWood, Color.Wheat });
            StandardReportsPieLight = new ColorPalette(new Color[] { Color.LightSteelBlue, Color.Coral, Color.PaleTurquoise, Color.Yellow, Color.LightPink, Color.Gray });
            StandardReportsPieDark = new ColorPalette(new Color[] { Color.YellowGreen, Color.Maroon, Color.Purple, Color.DarkOrange, Color.DarkBlue, Color.DarkCyan, Color.DarkKhaki, Color.HotPink, Color.DarkSalmon, Color.LightGreen});
            MarketplaceAnalytics = CustomerContactReports;
            FormularyHistoryReporting = new ColorPalette(new Color[] { Color.RoyalBlue, Color.Indigo, Color.YellowGreen, Color.Olive, Color.DeepPink, Color.DarkCyan, Color.Goldenrod, Color.Brown, Color.MediumSeaGreen, Color.Sienna, Color.DarkGoldenrod, Color.OrangeRed });
        }

        public static ColorPalette StandardReports101 { get; private set; }
        public static ColorPalette StandardReportsPieLight { get; private set; }
        public static ColorPalette StandardReportsPieDark { get; private set; }
        public static ColorPalette StandardReports { get; private set; }
        public static ColorPalette CustomerContactReports { get; private set; }
        public static ColorPalette MarketplaceAnalytics { get; private set; }
        public static ColorPalette FormularyHistoryReporting { get; private set; }
    }
}