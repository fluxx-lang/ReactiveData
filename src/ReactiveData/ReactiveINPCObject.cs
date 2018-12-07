using System;
using System.ComponentModel;

namespace ReactiveData
{
    public sealed class ReactiveINPCObject<TValue> : ReactiveChangeable<TValue>, IDisposable where TValue : INotifyPropertyChanged
    {
        private readonly TValue _value;

        public ReactiveINPCObject(TValue value)
        {
            _value = value;
            _value.PropertyChanged += OnPropertyChanged;
        }

        public void Dispose()
        {
            _value.PropertyChanged -= OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
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
