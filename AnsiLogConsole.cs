using System;
using System.IO;

namespace Pillsgood.Extensions.Logging
{
    public class AnsiLogConsole : IConsole
    {
        private readonly TextWriter _textWriter;

        public AnsiLogConsole(bool stdErr = false)
        {
            _textWriter = stdErr ? Console.Error : Console.Out;
        }

        public void Write(string message)
        {
            _textWriter.Write(message);
        }
    }
}