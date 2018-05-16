using System;

namespace ReactiveData
{
    public static class ObjectExtensions
    {
        public static T Reactive<T>(this T obj, Action<T> action)
        {
            new ReactiveCode(() => action.Invoke(obj));
            return obj;
        }
    }
}