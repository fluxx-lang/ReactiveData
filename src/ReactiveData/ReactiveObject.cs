using System;

namespace ReactiveData
{
    public sealed class ReactiveObject<TValue> : ReactiveMutable<TValue>, IDisposable where TValue : INotifyObjectChanged
    {
        private readonly TValue _value;

        public ReactiveObject(TValue value)
        {
            _value = value;
            _value.ObjectChanged += OnObjectChanged;
        }

        public void Dispose()
        {
            _value.ObjectChanged -= OnObjectChanged;
        }

        private void OnObjectChanged()
        {
            NotifyChanged();
        }

        public override TValue Value {
            get {
                RunningDerivationsStack.Top?.AddDependency(this);
                return _value;
            }
        }
    }
}
