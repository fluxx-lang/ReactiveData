using System;
using System.ComponentModel;

namespace ReactiveData
{
    public static class ReactiveFactory
    {
        public static Reactive<TValue> ReactiveExpression<TValue>(Func<TValue> expressionFunction)
        {
            return new ReactiveExpression<TValue>(expressionFunction);
        }

        public static ReactiveVar<TValue> ReactiveVar<TValue>(TValue initialValue)
        {
            return new ReactiveVar<TValue>(initialValue);
        }

        public static Reactive<TValue> ReactiveConstant<TValue>(TValue constantValue)
        {
            return new ReactiveConstant<TValue>(constantValue);
        }

        public static ReactiveInpc<TValue> ReactiveInpc<TValue>(TValue inpcObject) where TValue : INotifyPropertyChanged
        {
            return new ReactiveInpc<TValue>(inpcObject);
        }
    }
}
