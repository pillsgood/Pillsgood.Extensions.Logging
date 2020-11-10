#nullable enable
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Pillsgood.Extensions.Logging
{
    public class ColorMessageBuilder
    {
        private const char SpaceChar = ' ';
        private readonly StringBuilder _stringBuilder = new StringBuilder();
        private readonly Queue<Action<StringBuilder>> _messageQueue = new Queue<Action<StringBuilder>>();

        internal string Message
        {
            get
            {
                for (var i = 0; i < _messageQueue.Count; i++)
                {
                    _messageQueue.Dequeue().Invoke(_stringBuilder);
                }

                return _stringBuilder.ToString();
            }
        }

        internal ColorMessageBuilder()
        {
        }

        public ColorMessageBuilder Append(bool value)
        {
            _messageQueue.Enqueue(builder => builder.Append(value));
            return this;
        }

        public ColorMessageBuilder Append(byte value)
        {
            _messageQueue.Enqueue(builder => builder.Append(value));
            return this;
        }

        public ColorMessageBuilder Append(char value)
        {
            _messageQueue.Enqueue(builder => builder.Append(value));
            return this;
        }

        [CLSCompliant(false)]
        public unsafe ColorMessageBuilder Append(char* value, int repeatCount)
        {
            _messageQueue.Enqueue(builder => builder.Append(value, repeatCount));
            return this;
        }

        public ColorMessageBuilder Append(char value, int repeatCount)
        {
            _messageQueue.Enqueue(builder => builder.Append(value, repeatCount));
            return this;
        }

        public ColorMessageBuilder Append(char[]? value)
        {
            _messageQueue.Enqueue(builder => builder.Append(value));
            return this;
        }

        public ColorMessageBuilder Append(char[]? value, int start, int count)
        {
            _messageQueue.Enqueue(builder => builder.Append(value, start, count));
            return this;
        }

        public ColorMessageBuilder Append(decimal value)
        {
            _messageQueue.Enqueue(builder => builder.Append(value));
            return this;
        }

        public ColorMessageBuilder Append(double value)
        {
            _messageQueue.Enqueue(builder => builder.Append(value));
            return this;
        }

        public ColorMessageBuilder Append(short value)
        {
            _messageQueue.Enqueue(builder => builder.Append(value));
            return this;
        }

        public ColorMessageBuilder Append(int value)
        {
            _messageQueue.Enqueue(builder => builder.Append(value));
            return this;
        }

        public ColorMessageBuilder Append(long value)
        {
            _messageQueue.Enqueue(builder => builder.Append(value));
            return this;
        }

        public ColorMessageBuilder Append(object? value)
        {
            _messageQueue.Enqueue(builder => builder.Append(value));
            return this;
        }

        public ColorMessageBuilder Append(ReadOnlyMemory<char> value)
        {
            _messageQueue.Enqueue(builder => builder.Append(value));
            return this;
        }

        public ColorMessageBuilder Append(ReadOnlySpan<char> value)
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

        public ColorMessageBuilder Append(sbyte value)
        {
            _messageQueue.Enqueue(builder => builder.Append(value));
            return this;
        }

        public ColorMessageBuilder Append(float value)
        {
            _messageQueue.Enqueue(builder => builder.Append(value));
            return this;
        }

        public ColorMessageBuilder Append(string? value)
        {
            _messageQueue.Enqueue(builder => builder.Append(value));
            return this;
        }

        public ColorMessageBuilder Append(string? value, int start, int count)
        {
            _messageQueue.Enqueue(builder => builder.Append(value, start, count));
            return this;
        }

        public ColorMessageBuilder Append(ushort value)
        {
            _messageQueue.Enqueue(builder => builder.Append(value));
            return this;
        }

        public ColorMessageBuilder Append(uint value)
        {
            _messageQueue.Enqueue(builder => builder.Append(value));
            return this;
        }

        public ColorMessageBuilder Append(ulong value)
        {
            _messageQueue.Enqueue(builder => builder.Append(value));
            return this;
        }


        public ColorMessageBuilder AppendJoin(string value)
        {
            _messageQueue.Enqueue(builder => builder.Append(SpaceChar).Append(value));
            return this;
        }

        public ColorMessageBuilder AppendJoin(char separator, params object?[] value)
        {
            _messageQueue.Enqueue(builder => builder.AppendJoin(separator, value));
            return this;
        }

        public ColorMessageBuilder AppendJoin(char separator, params string?[] value)
        {
            _messageQueue.Enqueue(builder => builder.AppendJoin(separator, value));
            return this;
        }

        public ColorMessageBuilder AppendJoin(string? separator, params object?[] value)
        {
            _messageQueue.Enqueue(builder => builder.AppendJoin(separator, value));
            return this;
        }

        public ColorMessageBuilder AppendJoin(string? separator, params string?[] value)
        {
            _messageQueue.Enqueue(builder => builder.AppendJoin(separator, value));
            return this;
        }

        public ColorMessageBuilder AppendJoin<T>(char separator, IEnumerable<T> value)
        {
            _messageQueue.Enqueue(builder => builder.AppendJoin(separator, value));
            return this;
        }

        public ColorMessageBuilder AppendJoin<T>(string? separator, IEnumerable<T> value)
        {
            _messageQueue.Enqueue(builder => builder.AppendJoin(separator, value));
            return this;
        }

        public ColorMessageBuilder AppendLine()
        {
            _messageQueue.Enqueue(builder => builder.AppendLine());
            return this;
        }

        public ColorMessageBuilder AppendLine(string? message)
        {
            _messageQueue.Enqueue(builder => builder.AppendLine(message));
            return this;
        }


        public ColorMessageBuilder AppendFormat(IFormatProvider provider, string format, object? arg0)
        {
            _messageQueue.Enqueue(builder => builder.AppendFormat(provider, format, arg0));
            return this;
        }

        public ColorMessageBuilder AppendFormat(IFormatProvider provider, string format, object? arg0, object? arg1)
        {
            _messageQueue.Enqueue(builder => builder.AppendFormat(provider, format, arg0, arg1));
            return this;
        }

        public ColorMessageBuilder AppendFormat(IFormatProvider provider, string format, object? arg0, object? arg1,
            object? arg2)
        {
            _messageQueue.Enqueue(builder => builder.AppendFormat(provider, format, arg0, arg1, arg2));
            return this;
        }

        public ColorMessageBuilder AppendFormat(IFormatProvider? provider, string format, params object?[] args)
        {
            _messageQueue.Enqueue(builder => builder.AppendFormat(provider, format, args));
            return this;
        }

        public ColorMessageBuilder AppendFormat(string format, object? arg0, object? arg1)
        {
            _messageQueue.Enqueue(builder => builder.AppendFormat(format, arg0, arg1));
            return this;
        }

        public ColorMessageBuilder AppendFormat(string format, object? arg0, object? arg1, object? arg2)
        {
            _messageQueue.Enqueue(builder => builder.AppendFormat(format, arg0, arg1, arg2));
            return this;
        }

        public ColorMessageBuilder AppendFormat(string format, params object?[] args)
        {
            _messageQueue.Enqueue(builder => builder.AppendFormat(format, args));
            return this;
        }

        public ColorMessageBuilder Color(ConsoleColor? foreground = null, ConsoleColor? background = null)
        {
            for (var i = 1; i < _messageQueue.Count; i++)
            {
                _messageQueue.Dequeue().Invoke(_stringBuilder);
            }

            _stringBuilder.WriteWithColor(_messageQueue.Dequeue(), foreground, background);
            return this;
        }
    }
}