using System;
using System.Drawing;

namespace Pillsgood.Extensions.Logging
{
    internal class ColorUtilities
    {
        internal static ConsoleColor? GetFallbackConsoleColor(Color? color)
        {
            if (color.HasValue)
            {
                var hsl = new HSL(color.Value);
                ConsoleColor? consoleColor = null;
                if (hsl.Luminosity >= 0.9f)
                {
                    return ConsoleColor.White;
                }

                if (hsl.Luminosity <= 0.1f)
                {
                    return ConsoleColor.Black;
                }

                if (hsl.Saturation <= 0.15f)
                {
                    consoleColor = ConsoleColor.DarkGray;
                }

                consoleColor ??= ExamineHue(hsl.Hue);
                consoleColor = ExamineLuminosity(hsl.Luminosity, consoleColor.Value);
                return consoleColor;
            }

            return null;
        }

        private static ConsoleColor ExamineLuminosity(float luminosity, ConsoleColor consoleColor)
        {
            if (luminosity > 0.1f && luminosity < 0.5f)
            {
                return GetDarkColor(consoleColor);
            }

            if (consoleColor == ConsoleColor.DarkGray && luminosity >= 0.7f)
            {
                return ConsoleColor.Gray;
            }

            return consoleColor;
        }

        internal static ConsoleColor GetDarkColor(ConsoleColor color)
        {
            return color switch
            {
                ConsoleColor.Gray => ConsoleColor.DarkGray,
                ConsoleColor.Blue => ConsoleColor.DarkBlue,
                ConsoleColor.Green => ConsoleColor.DarkGreen,
                ConsoleColor.Cyan => ConsoleColor.DarkCyan,
                ConsoleColor.Red => ConsoleColor.DarkRed,
                ConsoleColor.Magenta => ConsoleColor.DarkMagenta,
                ConsoleColor.Yellow => ConsoleColor.DarkYellow,
                _ => color
            };
        }


        private static ConsoleColor ExamineHue(float hue)
        {
            return hue switch
            {
                >= 0 and < 15 => ConsoleColor.Red,
                >= 15 and < 45 => ConsoleColor.DarkYellow,
                >= 45 and < 70 => ConsoleColor.Yellow,
                >= 70 and < 150 => ConsoleColor.Green,
                >= 150 and < 210 => ConsoleColor.Cyan,
                >= 210 and < 255 => ConsoleColor.Blue,
                >= 255 and < 345 => ConsoleColor.Magenta,
                >= 345 => ConsoleColor.Red,
                _ => ConsoleColor.White
            };
        }

        private class HSL
        {
            public float Hue { get; }

            public float Saturation { get; }

            public float Luminosity { get; }

            public HSL(Color color)
            {
                var r = color.R / 255f;
                var g = color.G / 255f;
                var b = color.B / 255f;

                var min = Math.Min(Math.Min(r, g), b);
                var max = Math.Max(Math.Max(r, g), b);
                var delta = max - min;

                Hue = 0f;
                Saturation = 0f;
                Luminosity = (max + min) / 2.0f;

                if (delta != 0)
                {
                    Saturation = Luminosity < 0.5f ? delta / (max + min) : delta / (2.0f - max - min);


                    if (Math.Abs(r - max) < 0.1)
                    {
                        Hue = (g - b) / delta;
                    }
                    else if (Math.Abs(g - max) < 0.1)
                    {
                        Hue = 2f + (b - r) / delta;
                    }
                    else if (Math.Abs(b - max) < 0.1)
                    {
                        Hue = 4f + (r - g) / delta;
                    }
                }

                Hue *= 60f;
                if (Hue < 0) Hue += 360;
                Hue %= 360;
            }
        }
    }
}