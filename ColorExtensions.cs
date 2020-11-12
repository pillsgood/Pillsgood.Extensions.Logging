using System;
using System.Drawing;

namespace Pillsgood.Extensions.Logging
{
    internal static class ColorExtensions
    {
        internal static ConsoleColor? GetFallbackConsoleColor(this Color color)
        {
            ConsoleColor? consoleColor = null;
            var brightness = color.GetBrightness();
            if (brightness >= 0.9f)
            {
                return ConsoleColor.White;
            }

            if (brightness <= 0.1f)
            {
                return ConsoleColor.Black;
            }

            if (color.GetSaturation() <= 0.15f)
            {
                consoleColor = ConsoleColor.DarkGray;
            }

            consoleColor ??= ExamineHue(color.GetHue());
            consoleColor = ExamineLuminosity(brightness, consoleColor.Value);
            return consoleColor;
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

        private static ConsoleColor GetDarkColor(ConsoleColor color)
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
    }
}