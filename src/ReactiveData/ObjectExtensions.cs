using System;

namespace ReactiveData
{
    public static class ObjectExtensions
    {
        public static T React<T>(this T obj, Action<T> action)
        {
            new ReactiveCode(() => action.Invoke(obj));
            return obj;
        }

        public static T React<T>(this T obj, Action action)
        {
            new ReactiveCode(action.Invoke);
            return obj;
        }
    }
}