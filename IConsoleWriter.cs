#nullable enable
using System;
using System.Text;

namespace Pillsgood.Extensions.Logging
{
    public interface IConsoleWriter
    {
        void WriteLine();
        void WriteLine(bool value);
        void WriteLine(char value);
        void WriteLine(char[] value);
        void WriteLine(char[] value, int index, int count);
        void WriteLine(decimal value);
        void WriteLine(double value);
        void WriteLine(int value);
        void WriteLine(long value);
        void WriteLine(object? value);
        void WriteLine(ReadOnlySpan<char> buffer);
        void WriteLine(string value);
        void WriteLine(string format, object? arg0);
        void WriteLine(string format, object? arg0, object? arg1);
        void WriteLine(string format, object? arg0, object? arg1, object? arg2);
        void WriteLine(string format, params object?[] arg);
        void WriteLine(StringBuilder value);
        void WriteLine(uint value);
        void WriteLine(ulong value);
        void WriteLine(Action<AnsiString> message);
        void Write(bool value);
        void Write(char value);
        void Write(char[] value);
        void Write(char[] value, int index, int count);
        void Write(decimal value);
        void Write(double value);
        void Write(int value);
        void Write(long value);
        void Write(object? value);
        void Write(ReadOnlySpan<char> buffer);
        void Write(string value);
        void Write(string format, object? arg0);
        void Write(string format, object? arg0, object? arg1);
        void Write(string format, object? arg0, object? arg1, object? arg2);
        void Write(string format, params object?[] arg);
        void Write(StringBuilder value);
        void Write(uint value);
        void Write(ulong value);
        void Write(Action<AnsiString> message);
    }
}