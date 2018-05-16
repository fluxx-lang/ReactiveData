using System;
using System.Collections.Generic;

namespace ReactiveData {
    public abstract class ReactiveChangableData<TValue> : IReactive<TValue> {
        public virtual event ReactiveDataChangedEventHandler ReactiveDataChanged;

        public void NotifyChanged(State state)
        {
            ReactiveDataChanged?.Invoke(state);
        }

        public bool HaveSubscribers => ReactiveDataChanged != null;

        public abstract TValue CurrentValue { get; }

        public abstract TValue Value { get; }
    }
}
