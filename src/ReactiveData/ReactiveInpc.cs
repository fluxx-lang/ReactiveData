using System;
using System.ComponentModel;

namespace ReactiveData
{
    public sealed class ReactiveInpc<TValue> : Reactive<TValue>, IDisposable where TValue : INotifyPropertyChanged
    {
        private readonly TValue _value;

        public ReactiveInpc(TValue value)
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

    public static class INotifyPropertyChangedExtensions
    {
        public static ReactiveExpression<TExpression> ReactiveExpression<TObject, TExpression>(this TObject obj, Func<TObject, TExpression> expression) where TObject : INotifyPropertyChanged
        {
            var reactiveObj = new ReactiveInpc<TObject>(obj);
            return new ReactiveExpression<TExpression>(() => expression(reactiveObj.Value));
        }
    }
}
