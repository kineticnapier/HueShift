using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace HueShift
{
    class ColorConverter
    {
        //HSL structure
        //Hue: 0-360, Saturation: 0-1, Lightness: 0-1
        public struct HSL
        {
            public double H;
            public double S;
            public double L;
        }

        //System.Windows.Media.Color to HSL
        public static HSL RgbToHsl(Color color)
        {
            HSL hsl = new HSL();

            double r = color.R / 255.0;
            double g = color.G / 255.0;
            double b = color.B / 255.0;

            double max = Math.Max(r, Math.Max(g, b));
            double min = Math.Min(r, Math.Min(g, b));
            double delta = max - min;

            //Lightness
            hsl.L = (max + min) / 2.0;

            if (delta == 0)
            {
                // Monochromatic
                hsl.H = 0;
                hsl.S = 0;
            }
            else
            {
                //Saturation
                hsl.S = (hsl.L < 0.5) ? (delta / (max + min)) : (delta / (2.0 - max - min));

                //Hue
                if (max == r)
                {
                    hsl.H = (g - b) / delta;
                }
                else if (max == g)
                {
                    hsl.H = (b - r) / delta + 2.0;
                }
                else
                {
                    hsl.H = (r - g) / delta + 4.0;
                }

                hsl.H *= 60;
                if (hsl.H < 0)
                {
                    hsl.H += 360;
                }
            }
            return hsl;
        }

        //HSL to System.Windows.Media.Color
        public static Color HslToRgb(HSL hsl)
        {
            byte r, g, b;

            if (hsl.S == 0)
            {
                // Monochromatic
                r = g = b = (byte)(hsl.L * 255);
            }
            else
            {
                double q = hsl.L < 0.5 ? (hsl.L * (1.0 + hsl.S)) : (hsl.L + hsl.S - hsl.L * hsl.S);
                double p = 2.0 * hsl.L - q;
                r = (byte)Math.Round(HueToRgb(p, q, hsl.H + 120) * 255.0);
                g = (byte)Math.Round(HueToRgb(p, q, hsl.H) * 255.0);
                b = (byte)Math.Round(HueToRgb(p, q, hsl.H - 120) * 255.0);
            }

            return Color.FromRgb(r, g, b);
        }

        private static double HueToRgb(double p, double q, double t)
        {
            if (t < 0) t += 360;
            if (t > 360) t -= 360;
            if (t < 60) return p + (q - p) * t / 60;
            if (t < 180) return q;
            if (t < 240) return p + (q - p) * (240 - t) / 60;
            return p;
        }

        //Complementary color in RGB
        public static Color GetComplementaryColor(Color color)
        {
            HSL hsl = RgbToHsl(color);
            hsl.H = (hsl.H + 180) % 360;
            return HslToRgb(hsl);
        }

        //Invert color in RGB
        public static Color GetInvertColor(Color color)
        {
            return Color.FromRgb(
                (byte)(255 - color.R), 
                (byte)(255 - color.G), 
                (byte)(255 - color.B)
            );
        }

        //Convert to Gray scale
        public static Color GetGrayscaleColor(Color color)
        {
            double r = color.R / 255.0;
            double g = color.G / 255.0;
            double b = color.B / 255.0;

            //BT.709
            double luminance = 0.2126 * r + 0.7152 * g + 0.0722 * b;

            byte grayValue = (byte)Math.Round(luminance * 255.0);

            return Color.FromRgb(grayValue, grayValue, grayValue);
        }
    }
}
