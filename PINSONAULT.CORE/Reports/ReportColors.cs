using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;


namespace Pinsonault.Data.Reports
{
    public class ColorPalette
    {
        public IEnumerable<Color> Colors;

        public static ColorPalette Default { get; private set; }

        static ColorPalette()
        {
            Default = new ColorPalette(new Color[] { Color.FromArgb(248, 206, 12), Color.FromArgb(0, 202, 56), Color.FromArgb(16, 100, 255), Color.FromArgb(255, 118, 18), Color.FromArgb(128, 4, 255), Color.FromArgb(0, 195, 224), Color.FromArgb(237, 0, 38), Color.FromArgb(13, 103, 155) });
        }

        public ColorPalette(IEnumerable<Color> Colors)
        {
            this.Colors = Colors;
        }

        public Color GetColor(int index)
        {
            return GetColor(index, Colors);
        }
        public string GetColorAsHexString(int index)
        {
            return "#" + GetColor(index, Colors).ToArgb().ToString("x").Substring(2);
        }
        public string GetLightColorAsHexString(int index)
        {
            return "#" + GetColor(index, Colors).ToArgb().ToString("x").Substring(2);
        }

        public static Color GetColor(int index, IEnumerable<Color> colors)
        {
            //if ( index >= 0 && index < colors.Count() )
            //    
            int colorIndex = index % colors.Count();
            
            return colors.Skip(colorIndex).FirstOrDefault();
            //return Color.Black;
        }
    }
}
