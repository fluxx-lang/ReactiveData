namespace ReactiveData.Sequence
{
    public class ItemsSequence<T> : IItemsSequence<T>
    {
        private readonly T[] _items;

        public ItemsSequence(T[] items)
        {
            _items = items;
        }

        public T[] Items => _items;

        public int ItemCount => _items.Length;
    }
}
