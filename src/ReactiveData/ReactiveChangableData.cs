using System;
using System.Collections.Generic;

namespace ReactiveData {
    public abstract class ReactiveChangableData<TValue> : IReactive<TValue> {
        public virtual event ReactiveDataChangedEventHandler DataChanged;

        public void NotifyChanged(State state)
        {
            DataChanged?.Invoke(state);
        }

        public bool HaveSubscribers => DataChanged != null;

        public abstract TValue CurrentValue { get; }

        public abstract TValue Value { get; }
    }
}
