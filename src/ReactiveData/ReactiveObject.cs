using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ReactiveData {
    public abstract class ReactiveObject : ReactiveBase
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected T Get<T>(T storage)
        {
            RunningDerivationsStack.Top?.AddDependency(this);
            return storage;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void Set<T>(out T storage, T value)
        {
            storage = value;
            NotifyChanged();
        }
    }
}
