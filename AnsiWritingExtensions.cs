using System;
using System.Drawing;
using System.IO;
using System.Text;

namespace Pillsgood.Extensions.Logging
{
    internal static class AnsiWritingExtensions
    {
        private const string DefaultForegroundColor = "\x1B[39m\x1B[22m";
        private const string DefaultBackgroundColor = "\x1B[49m";

        private static string GetForegroundColorEscapeCode(Color color) =>
            $"\x1B[38;2;{color.R:000};{color.G:000};{color.B:000}m";


        private static string GetBackgroundColorEscapeCode(Color color) =>
            $"\x1B[48;2;{color.R:000};{color.G:000};{color.B:000}m";

        private static string GetForegroundColorEscapeCode(ConsoleColor color)
        {
            return color switch
            {
                ConsoleColor.Black => "\x1B[30m",
                ConsoleColor.DarkRed => "\x1B[31m",
                ConsoleColor.DarkGreen => "\x1B[32m",
                ConsoleColor.DarkYellow => "\x1B[33m",
                ConsoleColor.DarkBlue => "\x1B[34m",
                ConsoleColor.DarkMagenta => "\x1B[35m",
                ConsoleColor.DarkCyan => "\x1B[36m",
                ConsoleColor.Gray => "\x1B[37m",
                ConsoleColor.DarkGray => "\x1B[1m\x1B[30m",
                ConsoleColor.Red => "\x1B[1m\x1B[31m",
                ConsoleColor.Green => "\x1B[1m\x1B[32m",
                ConsoleColor.Yellow => "\x1B[1m\x1B[33m",
                ConsoleColor.Blue => "\x1B[1m\x1B[34m",
                ConsoleColor.Magenta => "\x1B[1m\x1B[35m",
                ConsoleColor.Cyan => "\x1B[1m\x1B[36m",
                ConsoleColor.White => "\x1B[1m\x1B[37m",
                _ => DefaultForegroundColor
            };
        }

        private static string GetBackgroundColorEscapeCode(ConsoleColor color)
        {
            return color switch
            {
                ConsoleColor.Black => "\x1B[40m",
                ConsoleColor.DarkRed => "\x1B[41m",
                ConsoleColor.DarkGreen => "\x1B[42m",
                ConsoleColor.DarkYellow => "\x1B[43m",
                ConsoleColor.DarkBlue => "\x1B[44m",
                ConsoleColor.DarkMagenta => "\x1B[45m",
                ConsoleColor.DarkCyan => "\x1B[46m",
                ConsoleColor.Gray => "\x1B[47m",
                ConsoleColor.DarkGray => "\x1B[1m\x1B[40m",
                ConsoleColor.Red => "\x1B[1m\x1B[41m",
                ConsoleColor.Green => "\x1B[1m\x1B[42m",
                ConsoleColor.Yellow => "\x1B[1m\x1B[43m",
                ConsoleColor.Blue => "\x1B[1m\x1B[44m",
                ConsoleColor.Magenta => "\x1B[1m\x1B[45m",
                ConsoleColor.Cyan => "\x1B[1m\x1B[46m",
                ConsoleColor.White => "\x1B[1m\x1B[47m",
                _ => DefaultBackgroundColor
            };
        }


        public static void WriteColoredMessage(this StringBuilder stringBuilder, Action<StringBuilder> invoker,
            Color? foreground, Color? background)
        {
            if (background.HasValue) stringBuilder.Append(GetBackgroundColorEscapeCode(background.Value));

            if (foreground.HasValue) stringBuilder.Append(GetForegroundColorEscapeCode(foreground.Value));

            invoker.Invoke(stringBuilder);

            if (foreground.HasValue)
            {
                stringBuilder.Replace(DefaultForegroundColor, GetForegroundColorEscapeCode(foreground.Value));
                stringBuilder.Append(DefaultForegroundColor);
            }

            if (background.HasValue)
            {
                stringBuilder.Replace(DefaultBackgroundColor, GetBackgroundColorEscapeCode(background.Value));
                stringBuilder.Append(DefaultBackgroundColor);
            }
        }
        


        public static void WriteColoredMessage(this StringBuilder stringBuilder, Action<StringBuilder> invoker,
            ConsoleColor? foreground, ConsoleColor? background)
        {
            if (background.HasValue) stringBuilder.Append(GetBackgroundColorEscapeCode(background.Value));

            if (foreground.HasValue) stringBuilder.Append(GetForegroundColorEscapeCode(foreground.Value));

            invoker.Invoke(stringBuilder);

            if (foreground.HasValue)
            {
                stringBuilder.Replace(DefaultForegroundColor, GetForegroundColorEscapeCode(foreground.Value));
                stringBuilder.Append(DefaultForegroundColor);
            }

            if (background.HasValue)
            {
                stringBuilder.Replace(DefaultBackgroundColor, GetBackgroundColorEscapeCode(background.Value));
                stringBuilder.Append(DefaultBackgroundColor);
            }

        }
        

        public static void WriteColoredMessage(this TextWriter textWriter, string message,
            Color? foreground = null,
            Color? background = null)
        {
            if (background.HasValue) textWriter.Write(GetBackgroundColorEscapeCode(background.Value));

            if (foreground.HasValue) textWriter.Write(GetForegroundColorEscapeCode(foreground.Value));

            textWriter.Write(message);

            if (foreground.HasValue) textWriter.Write(DefaultForegroundColor);

            if (background.HasValue) textWriter.Write(DefaultBackgroundColor);
        }
    }
}