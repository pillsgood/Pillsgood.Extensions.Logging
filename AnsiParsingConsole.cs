using System;
using System.IO;

namespace Pillsgood.Extensions.Logging
{
    internal class AnsiParsingLogConsole : IConsole
    {
        private readonly TextWriter _textWriter;
        private readonly AnsiParser _parser;

        public AnsiParsingLogConsole(bool stdErr = false)
        {
            _textWriter = stdErr ? Console.Error : Console.Out;
            _parser = new AnsiParser(WriteToConsole);
        }

        public void Write(string message)
        {
            _parser.Parse(message);
        }

        private static bool SetColor(ConsoleColor? background, ConsoleColor? foreground)
        {
            var backgroundChanged = SetBackgroundColor(background);
            return SetForegroundColor(foreground) || backgroundChanged;
        }

        private static bool SetBackgroundColor(ConsoleColor? background)
        {
            if (background.HasValue)
            {
                Console.BackgroundColor = background.Value;
                return true;
            }

            return false;
        }

        private static bool SetForegroundColor(ConsoleColor? foreground)
        {
            if (foreground.HasValue)
            {
                Console.ForegroundColor = foreground.Value;
                return true;
            }

            return false;
        }

        private static void ResetColor() => Console.ResetColor();

        private void WriteToConsole(string message, int startIndex, int length, ConsoleColor? background,
            ConsoleColor? foreground)
        {
            var span = message.AsSpan().Slice(startIndex, length);
            var colorChanged = SetColor(background, foreground);
#if NETCOREAPP
            _textWriter.Write(span);
#else
            _textWriter.Write(span.ToString());
#endif
            if (colorChanged)
            {
                ResetColor();
            }
        }
    }
}