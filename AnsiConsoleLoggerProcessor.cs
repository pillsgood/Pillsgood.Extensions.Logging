using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Pillsgood.Extensions.Logging
{
    internal class AnsiConsoleLoggerProcessor : IDisposable
    {
        private const int _maxQueuedMessages = 1024;

        private readonly BlockingCollection<LogMessageEntry> _messageQueue = new BlockingCollection<LogMessageEntry>();
        private readonly Thread _outputThread;

        public IConsole Console;
        public IConsole ErrorConsole;

        public AnsiConsoleLoggerProcessor()
        {
            _outputThread = new Thread(ProcessLogQueue)
            {
                IsBackground = true,
                Name = "Console logger queue processing thread"
            };
            _outputThread.Start();
        }

        public virtual void EnqueueMessage(LogMessageEntry message)
        {
            if (!_messageQueue.IsAddingCompleted)
            {
                try
                {
                    _messageQueue.Add(message);
                    return;
                }
                catch (InvalidOperationException)
                {
                }
            }

            try
            {
                WriteMessage(message);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        internal virtual void WriteMessage(LogMessageEntry entry)
        {
            var console = entry.LogAsError ? ErrorConsole : Console;
            console.Write(entry.Message);
        }

        private void ProcessLogQueue()
        {
            try
            {
                foreach (var message in _messageQueue.GetConsumingEnumerable())
                {
                    WriteMessage(message);
                }
            }
            catch
            {
                try
                {
                    _messageQueue.CompleteAdding();
                }
                catch
                {
                    // ignored
                }
            }
        }

        public void Dispose()
        {
            _messageQueue.CompleteAdding();
            try
            {
                _outputThread.Join(1500);
            }
            catch (ThreadStateException)
            {
            }
        }
    }
}