#nullable enable
using System;
using System.Buffers;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Pillsgood.Extensions.Logging
{
    internal class AnsiConsoleWriter : IConsoleWriter
    {
        private readonly AnsiConsoleLoggerProcessor _queueProcessor;
        [ThreadStatic] private static StringWriter? _stringWriter;

        public AnsiConsoleWriter(AnsiConsoleLoggerProcessor queueProcessor)
        {
            _queueProcessor = queueProcessor;
        }

        private async void WriteToStream(Action<TextWriter> handle)
        {
            _stringWriter ??= new StringWriter();
            handle.Invoke(_stringWriter);
            var sb = _stringWriter.GetStringBuilder();
            if (sb.Length == 0) return;
            var computedAnsiString = sb.ToString();
            sb.Clear();
            if (sb.Capacity > 1024)
            {
                sb.Capacity = 1024;
            }

            _queueProcessor.EnqueueMessage(new LogMessageEntry(computedAnsiString));
            await Task.Delay(5);
        }

        public void WriteLine()
        {
            WriteToStream(writer => writer.WriteLine());
        }

        public void WriteLine(bool value)
        {
            WriteToStream(writer => writer.WriteLine(value));
        }

        public void WriteLine(char value)
        {
            WriteToStream(writer => writer.WriteLine(value));
        }

        public void WriteLine(char[] value)
        {
            WriteToStream(writer => writer.WriteLine(value));
        }

        public void WriteLine(char[] value, int index, int count)
        {
            WriteToStream(writer => writer.WriteLine(value, index, count));
        }

        public void WriteLine(decimal value)
        {
            WriteToStream(writer => writer.WriteLine(value));
        }

        public void WriteLine(double value)
        {
            WriteToStream(writer => writer.WriteLine(value));
        }

        public void WriteLine(int value)
        {
            WriteToStream(writer => writer.WriteLine(value));
        }

        public void WriteLine(long value)
        {
            WriteToStream(writer => writer.WriteLine(value));
        }

        public void WriteLine(object? value)
        {
            WriteToStream(writer => writer.WriteLine(value));
        }

        public void WriteLine(ReadOnlySpan<char> buffer)
        {
            char[] array = ArrayPool<char>.Shared.Rent(buffer.Length);

            try
            {
                buffer.CopyTo(new Span<char>(array));
                var bufferLength = buffer.Length;

                WriteToStream(writer => writer.WriteLine(array, 0, bufferLength));
            }
            finally
            {
                ArrayPool<char>.Shared.Return(array);
            }
        }

        public void WriteLine(string value)
        {
            WriteToStream(writer => writer.WriteLine(value));
        }

        public void WriteLine(string format, object? arg0)
        {
            WriteToStream(writer => writer.WriteLine(format, arg0));
        }

        public void WriteLine(string format, object? arg0, object? arg1)
        {
            WriteToStream(writer => writer.WriteLine(format, arg0, arg1));
        }

        public void WriteLine(string format, object? arg0, object? arg1, object? arg2)
        {
            WriteToStream(writer => writer.WriteLine(format, arg0, arg1, arg2));
        }

        public void WriteLine(string format, params object?[] arg)
        {
            WriteToStream(writer => writer.WriteLine(format, arg));
        }

        public void WriteLine(StringBuilder value)
        {
            WriteToStream(writer => writer.WriteLine(value));
        }

        public void WriteLine(uint value)
        {
            WriteToStream(writer => writer.WriteLine(value));
        }

        public void WriteLine(ulong value)
        {
            WriteToStream(writer => writer.WriteLine(value));
        }

        public void WriteLine(Action<AnsiString> message)
        {
            var builder = new AnsiString();
            message.Invoke(builder);
            WriteToStream(writer => writer.WriteLine(builder.Message));
        }

        public void Write(bool value)
        {
            WriteToStream(writer => writer.Write(value));
        }

        public void Write(char value)
        {
            WriteToStream(writer => writer.Write(value));
        }

        public void Write(char[] value)
        {
            WriteToStream(writer => writer.Write(value));
        }

        public void Write(char[] value, int index, int count)
        {
            WriteToStream(writer => writer.Write(value, index, count));
        }

        public void Write(decimal value)
        {
            WriteToStream(writer => writer.Write(value));
        }

        public void Write(double value)
        {
            WriteToStream(writer => writer.Write(value));
        }

        public void Write(int value)
        {
            WriteToStream(writer => writer.Write(value));
        }

        public void Write(long value)
        {
            WriteToStream(writer => writer.Write(value));
        }

        public void Write(object? value)
        {
            WriteToStream(writer => writer.Write(value));
        }

        public void Write(ReadOnlySpan<char> buffer)
        {
            char[] array = ArrayPool<char>.Shared.Rent(buffer.Length);

            try
            {
                buffer.CopyTo(new Span<char>(array));
                var bufferLength = buffer.Length;

                WriteToStream(writer => writer.Write(array, 0, bufferLength));
            }
            finally
            {
                ArrayPool<char>.Shared.Return(array);
            }
        }

        public void Write(string value)
        {
            WriteToStream(writer => writer.Write(value));
        }

        public void Write(string format, object? arg0)
        {
            WriteToStream(writer => writer.Write(format, arg0));
        }

        public void Write(string format, object? arg0, object? arg1)
        {
            WriteToStream(writer => writer.Write(format, arg0, arg1));
        }

        public void Write(string format, object? arg0, object? arg1, object? arg2)
        {
            WriteToStream(writer => writer.Write(format, arg0, arg1, arg2));
        }

        public void Write(string format, params object?[] arg)
        {
            WriteToStream(writer => writer.Write(format, arg));
        }

        public void Write(StringBuilder value)
        {
            WriteToStream(writer => writer.Write(value));
        }

        public void Write(uint value)
        {
            WriteToStream(writer => writer.Write(value));
        }

        public void Write(ulong value)
        {
            WriteToStream(writer => writer.Write(value));
        }

        public void Write(Action<AnsiString> message)
        {
            var builder = new AnsiString();
            message.Invoke(builder);
            WriteToStream(writer => writer.Write(builder.Message));
        }
    }
}