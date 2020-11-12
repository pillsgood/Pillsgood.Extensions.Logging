using System;
using System.Drawing;

namespace Pillsgood.Extensions.Logging
{
    internal class AnsiParser
    {
        private readonly Action<string, int, int, ConsoleColor?, ConsoleColor?> _onParseWrite;

        public AnsiParser(Action<string, int, int, ConsoleColor?, ConsoleColor?> onParseWrite)
        {
            _onParseWrite = onParseWrite ?? throw new ArgumentNullException(nameof(onParseWrite));
        }

        private static bool IsDigit(char c) => (uint) (c - '0') <= ('9' - '0');

        public void Parse(string message)
        {
            var startIndex = -1;
            var length = 0;
            ConsoleColor? foreground = null;
            ConsoleColor? background = null;
            var span = message.AsSpan();
            const char EscapeChar = '\x1B';
            var isBright = false;
            for (var i = 0; i < span.Length; i++)
            {
                if (ParseAnsiCode(message, span, EscapeChar, ref i, ref startIndex, ref length, ref background,
                    ref foreground, ref isBright)) continue;

                if (startIndex == -1)
                {
                    startIndex = i;
                }

                var nextEscapeIndex = -1;
                if (i < message.Length - 1)
                {
                    nextEscapeIndex = message.IndexOf(EscapeChar, i + 1);
                }

                if (nextEscapeIndex < 0)
                {
                    length = message.Length - startIndex;
                    break;
                }

                length = nextEscapeIndex - startIndex;
                i = nextEscapeIndex - 1;
            }

            if (startIndex != -1)
            {
                _onParseWrite(message, startIndex, length, background, foreground);
            }
        }

        private bool ParseAnsiCode(string message, ReadOnlySpan<char> span, char EscapeChar, ref int i,
            ref int startIndex,
            ref int length, ref ConsoleColor? background, ref ConsoleColor? foreground, ref bool isBright)
        {
            if (span[i] == EscapeChar && span.Length >= i + 4 && span[i + 1] == '[')
            {
                if (span[i + 5] == '2' && span[i + 18] == 'm')
                {
                    if (Parse24BitColor(message, span, ref i, ref startIndex, ref length, ref background,
                        ref foreground, ref isBright)) return true;
                }
                else if (span[i + 3] == 'm')
                {
                    if (ParseBrightCode(message, span, background, foreground, ref i, ref startIndex, ref length,
                        ref isBright)) return true;
                }
                else if (span.Length >= i + 5 && span[i + 4] == 'm')
                {
                    if (Parse4BitColor(message, span, ref i, ref startIndex, ref length, ref background,
                        ref foreground, ref isBright)) return true;
                }
            }

            return false;
        }

        private bool Parse24BitColor(string message, ReadOnlySpan<char> span, ref int i, ref int startIndex,
            ref int length,
            ref ConsoleColor? background, ref ConsoleColor? foreground, ref bool isBright)
        {
            // Example: \x1B[48;2;000;000;000m
            if (IsDigit(span[i + 2]) && IsDigit(span[i + 3]))
            {
                int escapeCode;
                ConsoleColor? color;
                escapeCode = (span[i + 2] - '0') * 10 + (span[i + 3] - '0');
                if (startIndex != -1)
                {
                    _onParseWrite(message, startIndex, length, background, foreground);
                    startIndex = -1;
                    length = 0;
                }

                var r = int.Parse(span[(i + 7)..(i + 10)]);
                var g = int.Parse(span[(i + 11)..(i + 14)]);
                var b = int.Parse(span[(i + 15)..(i + 18)]);
                color = ColorUtilities.GetFallbackConsoleColor(Color.FromArgb(r, g, b));
                switch (escapeCode)
                {
                    case 38:
                        foreground = color;
                        isBright = false;
                        break;
                    case 48:
                        background = color;
                        isBright = false;
                        break;
                }

                i += 18;
                return true;
            }

            return false;
        }

        private bool Parse4BitColor(string message, ReadOnlySpan<char> span, ref int i, ref int startIndex,
            ref int length,
            ref ConsoleColor? background, ref ConsoleColor? foreground, ref bool isBright)
        {
            // Example: \x1B[40m
            if (IsDigit(span[i + 2]) && IsDigit(span[i + 3]))
            {
                var escapeCode = (span[i + 2] - '0') * 10 + (span[i + 3] - '0');
                if (startIndex != -1)
                {
                    _onParseWrite(message, startIndex, length, background, foreground);
                    startIndex = -1;
                    length = 0;
                }

                ConsoleColor? color;
                if (escapeCode >= 30 && escapeCode < 40 &&
                    TryGetColor(escapeCode % 10, isBright, out color))
                {
                    foreground = color;
                    isBright = false;
                }
                else if (escapeCode >= 40 && escapeCode < 50 &&
                         TryGetColor(escapeCode % 10, isBright, out color))
                {
                    background = color;
                    isBright = false;
                }

                i += 4;
                return true;
            }

            return false;
        }

        private bool ParseBrightCode(string message, ReadOnlySpan<char> span, ConsoleColor? background,
            ConsoleColor? foreground,
            ref int i, ref int startIndex, ref int length, ref bool isBright)
        {
            // Example: \x1B[1m
            if (IsDigit(span[i + 2]))
            {
                var escapeCode = (int) (span[i + 2] - '0');
                if (startIndex != -1)
                {
                    _onParseWrite(message, startIndex, length, background, foreground);
                    startIndex = -1;
                    length = 0;
                }

                if (escapeCode == 1)
                    isBright = true;
                i += 3;
                return true;
            }

            return false;
        }

        private static bool TryGetColor(int number, bool isBright, out ConsoleColor? color)
        {
            color = number switch
            {
                0 => isBright ? ConsoleColor.DarkGray : ConsoleColor.Black,
                1 => isBright ? ConsoleColor.Red : ConsoleColor.DarkRed,
                2 => isBright ? ConsoleColor.Green : ConsoleColor.DarkGreen,
                3 => isBright ? ConsoleColor.Yellow : ConsoleColor.DarkYellow,
                4 => isBright ? ConsoleColor.Blue : ConsoleColor.DarkBlue,
                5 => isBright ? ConsoleColor.Magenta : ConsoleColor.DarkMagenta,
                6 => isBright ? ConsoleColor.Cyan : ConsoleColor.DarkCyan,
                7 => isBright ? ConsoleColor.White : ConsoleColor.Gray,
                _ => null
            };

            return color != null || number == 9;
        }
    }
}