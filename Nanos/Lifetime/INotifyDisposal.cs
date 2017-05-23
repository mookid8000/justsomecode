using System;

namespace Nanos.Lifetime
{
    public interface INotifyDisposal
    {
        event Action Disposed;
    }
}