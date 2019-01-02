using System;

namespace ReactiveData.Sequence
{
    public struct SequenceImmutableArray<T>
    {
        private static readonly SequenceImmutableArray<T> _emptyArray = new SequenceImmutableArray<T>(Array.Empty<T>());

        private readonly T[] _array;

        public static SequenceImmutableArray<T> Empty() => _emptyArray;

        public SequenceImmutableArray(T[] array)
        {
            _array = array;
        }

        public T this[int index] => _array[index];

        public int Count => _array.Length;

        internal T[] GetArray() => _array;
    }
}
