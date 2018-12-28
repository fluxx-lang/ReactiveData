using System;

namespace ReactiveData.Sequence
{
    public static class SequenceUtils
    {
        public static ItemsSequence<T> Items<T>(params T[] items)
        {
            return new ItemsSequence<T>(items);
        }

        public static ParentSequence<T> Sequences<T>(params ISequence<T>[] sequences)
        {
            return new ParentSequence<T>(sequences);
        }

        public static ExpressionReactiveSequence<T> Expression<T>(Func<INonreactiveSequence<T>> expressionFunction)
        {
            return new ExpressionReactiveSequence<T>(expressionFunction);
        }

        public static IfBuilder<T> If<T>(Func<bool> condition, params T[] items)
        {
            return null;
        }

        public class IfBuilder<T>
        {
            public IfBuilder<T> ElseIf(Func<bool> condition, params T[] items)
            {
                return null;
            }

            public ExpressionReactiveSequence<T> Else(params T[] items)
            {
                return null;
            }

            public ExpressionReactiveSequence<T> End()
            {
                return null;
            }

        }
    }
}