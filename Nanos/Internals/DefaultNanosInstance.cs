using System;
using Nanos.Lifetime;
using Serilog;

namespace Nanos.Internals
{
    class DefaultNanosInstance : IInitializable, INanosInstance, INotifyDisposal
    {
        readonly ILogger _logger = Log.ForContext<DefaultNanosInstance>();

        bool _disposed;

        public void Initialize()
        {
        }

        public void Dispose()
        {
            if (_disposed) return;

            try
            {
                Disposed?.Invoke();
            }
            finally
            {
                _disposed = true;
            }
        }

        public event Action Disposed;
    }
}