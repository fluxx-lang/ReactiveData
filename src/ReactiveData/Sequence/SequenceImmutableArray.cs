using System;

namespace ReactiveData.Sequence
{
    public struct SequenceImmutableArray<T>
    {
        // The nested class is needed to workaround the "causes a cycle in the struct layout" compiler error (bug?)
        private static class NestedWorkaround
        {
            public static readonly SequenceImmutableArray<T> _emptyArray = new SequenceImmutableArray<T>(Array.Empty<T>());
        }

        private readonly T[] _array;

        public static SequenceImmutableArray<T> Empty() => NestedWorkaround._emptyArray;

        public SequenceImmutableArray(T[] array)
        {
            _array = array;
        }

        public T this[int index] => _array[index];

        public int Count => _array.Length;

        internal T[] GetArray() => _array;
    }
}
