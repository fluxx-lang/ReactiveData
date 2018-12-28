using System;

namespace ReactiveData.Sequence
{
    public class EmptySequence<T> : IItemsSequence<T>
    {
        public T[] Items => Array.Empty<T>();

        public int ItemCount => 0;
    }
}
