using System;
using Microsoft.Extensions.Logging;

namespace Pillsgood.Extensions.Logging
{
    internal class NullExternalScopeProvider : IExternalScopeProvider
    {
        private NullExternalScopeProvider()
        {
        }

        public static IExternalScopeProvider Instance { get; } = new NullExternalScopeProvider();

        void IExternalScopeProvider.ForEachScope<TState>(Action<object, TState> callback, TState state)
        {
        }

        public IDisposable Push(object state) => NullScope.Instance;
    }
}