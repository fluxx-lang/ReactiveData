using System;
using System.Collections.Generic;
using ReactiveData.Sequence.IndexedList;

namespace ReactiveData.Sequence
{
    public static class SequenceUtils
    {
        /// <summary>
        /// Map a sequence to an IList. The list will be populated from the sequence when this method
        /// is called (by calling Clear and then Insert for each of the items in the sequence). And then
        /// the list will be updated whenever the underlying sequence changes (as much as possible with
        /// the minimum number of list operations). Essentially this makes an IList based list reactive.
        /// This method can be used, for instance, to map a sequence of UIElements to a UIElementCollection,
        /// updating the UI when any part of the sequence changes.
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="list">list implementing IList</param>
        /// <param name="sequence">sequence, which can be nonreactive or reactive</param>
        public static void ListOnSequence<T>(IList<T> list, ISequence<T> sequence)
        {
            IIndexedList<T> listInterfaceIndexedList = new ListInterfaceIndexedList<T>(list);
            new IndexedListOnSequence<T>(listInterfaceIndexedList, sequence);
        }

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