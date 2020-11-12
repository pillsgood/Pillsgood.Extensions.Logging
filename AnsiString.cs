#nullable enable
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace Pillsgood.Extensions.Logging
{
    public class AnsiString
    {
        private const char SpaceChar = ' ';
        private readonly StringBuilder _stringBuilder = new StringBuilder();
        private readonly Queue<Action<StringBuilder>> _messageQueue = new Queue<Action<StringBuilder>>();

        internal string Message
        {
            get
            {
                while (_messageQueue.Count > 0)
                {
                    _messageQueue.Dequeue().Invoke(_stringBuilder);
                }

                return _stringBuilder.ToString();
            }
        }

        internal AnsiString()
        {
        }

        public AnsiString Append(bool value)
        {
            _messageQueue.Enqueue(builder => builder.Append(value));
            return this;
        }

        public AnsiString Append(byte value)
        {
            _messageQueue.Enqueue(builder => builder.Append(value));
            return this;
        }

        public AnsiString Append(char value)
        {
            _messageQueue.Enqueue(builder => builder.Append(value));
            return this;
        }

        public unsafe AnsiString Append(char* value, int repeatCount)
        {
            _messageQueue.Enqueue(builder => builder.Append(value, repeatCount));
            return this;
        }

        public AnsiString Append(char value, int repeatCount)
        {
            _messageQueue.Enqueue(builder => builder.Append(value, repeatCount));
            return this;
        }

        public AnsiString Append(char[]? value)
        {
            _messageQueue.Enqueue(builder => builder.Append(value));
            return this;
        }

        public AnsiString Append(char[]? value, int start, int count)
        {
            _messageQueue.Enqueue(builder => builder.Append(value, start, count));
            return this;
        }

        public AnsiString Append(decimal value)
        {
            _messageQueue.Enqueue(builder => builder.Append(value));
            return this;
        }

        public AnsiString Append(double value)
        {
            _messageQueue.Enqueue(builder => builder.Append(value));
            return this;
        }

        public AnsiString Append(short value)
        {
            _messageQueue.Enqueue(builder => builder.Append(value));
            return this;
        }

        public AnsiString Append(int value)
        {
            _messageQueue.Enqueue(builder => builder.Append(value));
            return this;
        }

        public AnsiString Append(long value)
        {
            _messageQueue.Enqueue(builder => builder.Append(value));
            return this;
        }

        public AnsiString Append(object? value)
        {
            _messageQueue.Enqueue(builder => builder.Append(value));
            return this;
        }

        public AnsiString Append(ReadOnlyMemory<char> value)
        {
            _messageQueue.Enqueue(builder => builder.Append(value));
            return this;
        }

        public AnsiString Append(ReadOnlySpan<char> value)
        {
            if (value.Length > 0)
            {
                unsafe
                {
                    fixed (char* valueChars = &MemoryMarshal.GetReference(value))
                    {
                        Append(valueChars, value.Length);
                    }
                }
            }

            return this;
        }

        public AnsiString Append(sbyte value)
        {
            _messageQueue.Enqueue(builder => builder.Append(value));
            return this;
        }

        public AnsiString Append(float value)
        {
            _messageQueue.Enqueue(builder => builder.Append(value));
            return this;
        }

        public AnsiString Append(string? value)
        {
            _messageQueue.Enqueue(builder => builder.Append(value));
            return this;
        }

        public AnsiString Append(string? value, int start, int count)
        {
            _messageQueue.Enqueue(builder => builder.Append(value, start, count));
            return this;
        }

        public AnsiString Append(ushort value)
        {
            _messageQueue.Enqueue(builder => builder.Append(value));
            return this;
        }

        public AnsiString Append(uint value)
        {
            _messageQueue.Enqueue(builder => builder.Append(value));
            return this;
        }

        public AnsiString Append(ulong value)
        {
            _messageQueue.Enqueue(builder => builder.Append(value));
            return this;
        }


        public AnsiString AppendJoin(string value)
        {
            _messageQueue.Enqueue(builder => builder.Append(SpaceChar).Append(value));
            return this;
        }

        public AnsiString AppendJoin(char separator, params object?[] value)
        {
            _messageQueue.Enqueue(builder => builder.AppendJoin(separator, value));
            return this;
        }

        public AnsiString AppendJoin(char separator, params string?[] value)
        {
            _messageQueue.Enqueue(builder => builder.AppendJoin(separator, value));
            return this;
        }

        public AnsiString AppendJoin(string? separator, params object?[] value)
        {
            _messageQueue.Enqueue(builder => builder.AppendJoin(separator, value));
            return this;
        }

        public AnsiString AppendJoin(string? separator, params string?[] value)
        {
            _messageQueue.Enqueue(builder => builder.AppendJoin(separator, value));
            return this;
        }

        public AnsiString AppendJoin<T>(char separator, IEnumerable<T> value)
        {
            _messageQueue.Enqueue(builder => builder.AppendJoin(separator, value));
            return this;
        }

        public AnsiString AppendJoin<T>(string? separator, IEnumerable<T> value)
        {
            _messageQueue.Enqueue(builder => builder.AppendJoin(separator, value));
            return this;
        }

        public AnsiString AppendLine()
        {
            _messageQueue.Enqueue(builder => builder.AppendLine());
            return this;
        }

        public AnsiString AppendLine(string? message)
        {
            _messageQueue.Enqueue(builder => builder.AppendLine(message));
            return this;
        }


        public AnsiString AppendFormat(IFormatProvider provider, string format, object? arg0)
        {
            _messageQueue.Enqueue(builder => builder.AppendFormat(provider, format, arg0));
            return this;
        }

        public AnsiString AppendFormat(IFormatProvider provider, string format, object? arg0, object? arg1)
        {
            _messageQueue.Enqueue(builder => builder.AppendFormat(provider, format, arg0, arg1));
            return this;
        }

        public AnsiString AppendFormat(IFormatProvider provider, string format, object? arg0, object? arg1,
            object? arg2)
        {
            _messageQueue.Enqueue(builder => builder.AppendFormat(provider, format, arg0, arg1, arg2));
            return this;
        }

        public AnsiString AppendFormat(IFormatProvider? provider, string format, params object?[] args)
        {
            _messageQueue.Enqueue(builder => builder.AppendFormat(provider, format, args));
            return this;
        }

        public AnsiString AppendFormat(string format, object? arg0, object? arg1)
        {
            _messageQueue.Enqueue(builder => builder.AppendFormat(format, arg0, arg1));
            return this;
        }

        public AnsiString AppendFormat(string format, object? arg0, object? arg1, object? arg2)
        {
            _messageQueue.Enqueue(builder => builder.AppendFormat(format, arg0, arg1, arg2));
            return this;
        }

        public AnsiString AppendFormat(string format, params object?[] args)
        {
            _messageQueue.Enqueue(builder => builder.AppendFormat(format, args));
            return this;
        }

        public AnsiString Color(Color? foreground = null, Color? background = null)
        {
            for (var i = 1; i < _messageQueue.Count; i++)
            {
                _messageQueue.Dequeue().Invoke(_stringBuilder);
            }

            _stringBuilder.WriteColoredMessage(_messageQueue.Dequeue(), foreground, background);
            return this;
        }
        
        public AnsiString Color(ConsoleColor? foreground = null, ConsoleColor? background = null)
        {
            for (var i = 1; i < _messageQueue.Count; i++)
            {
                _messageQueue.Dequeue().Invoke(_stringBuilder);
            }

            _stringBuilder.WriteColoredMessage(_messageQueue.Dequeue(), foreground, background);
            return this;
        }

        public static string Build(Action<AnsiString> builder)
        {
            var ansiString = new AnsiString();
            builder(ansiString);
            return ansiString.Message;
        }
    }
}