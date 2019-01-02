using System;

namespace ReactiveData.Sequence
{
    public static class SequenceUtils
    {
        public static ItemsSequence<T> Items<T>(params T[] items)
        {
            return new ItemsSequence<T>(new SequenceImmutableArray<T>(items));
        }

        public static ParentSequence<T> Sequences<T>(params ISequence<T>[] sequences)
        {
            return new ParentSequence<T>(new SequenceImmutableArray<ISequence<T>>(sequences));
        }

        public static ExpressionReactiveSequence<T> Expression<T>(Func<INonreactiveSequence<T>> expressionFunction)
        {
            return new ExpressionReactiveSequence<T>(expressionFunction);
        }
    }
}