using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Pillsgood.Extensions.Logging
{
    internal class AnsiConsoleLoggerProcessor : IDisposable
    {
        private const int _maxQueuedMessages = 1024;

        private readonly BlockingCollection<LogMessageEntry> _messageQueue = new BlockingCollection<LogMessageEntry>();
        private readonly Thread _outputThread;

        public IConsole console;
        public IConsole errorConsole;
        private readonly Timer _timer;
        private const int TimeoutDuration = 500;

        public AnsiConsoleLoggerProcessor()
        {
            _outputThread = new Thread(ProcessLogQueue)
            {
                IsBackground = false,
                Name = "Console logger queue processing thread"
            };


            _timer = new Timer(Timeout, null, TimeoutDuration, 0);
            _outputThread.Start();
        }

        private void Timeout(object state)
        {
            _outputThread.IsBackground = true;
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
            var console = entry.LogAsError ? errorConsole : this.console;
            console.Write(entry.Message);
        }


        private void ProcessLogQueue()
        {
            try
            {
                foreach (var message in _messageQueue.GetConsumingEnumerable())
                {
                    WriteMessage(message);
                    _timer.Change(TimeoutDuration, 0);
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