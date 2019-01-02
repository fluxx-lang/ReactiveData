namespace ReactiveData.Sequence
{
    public class ItemsSequence<T> : IItemsSequence<T>
    {
        private readonly SequenceImmutableArray<T> _items;

        public ItemsSequence(SequenceImmutableArray<T> items)
        {
            _items = items;
        }

        public SequenceImmutableArray<T> Items => _items;

        public int ItemCount => _items.Count;
    }
}
