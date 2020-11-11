using System;
using System.Drawing;
using System.IO;
using System.Text;

namespace Pillsgood.Extensions.Logging
{
    internal static class ColorWritingExtensions
    {
        private const string DefaultForegroundColor = "\x1B[39m\x1B[22m";
        private const string DefaultBackgroundColor = "\x1B[49m";

        private static string GetForegroundColorEscapeCode(Color color) =>
            $"\x1B[38;2;{color.R};{color.G};{color.B}m";


        private static string GetBackgroundColorEscapeCode(Color color) =>
            $"\x1B[48;2;{color.R};{color.G};{color.B}m";


        public static void WriteColoredMessage(this StringBuilder stringBuilder, Action<StringBuilder> invoker,
            Color? foreground,
            Color? background)
        {
            if (background.HasValue) stringBuilder.Append(GetBackgroundColorEscapeCode(background.Value));

            if (foreground.HasValue) stringBuilder.Append(GetForegroundColorEscapeCode(foreground.Value));

            invoker.Invoke(stringBuilder);

            if (foreground.HasValue) stringBuilder.Append(DefaultForegroundColor);

            if (background.HasValue) stringBuilder.Append(DefaultBackgroundColor);
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