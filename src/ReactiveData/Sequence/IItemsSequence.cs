namespace ReactiveData.Sequence
{
    public interface IItemsSequence<T> : INonreactiveSequence<T>
    {
        T[] Items { get; }

        int ItemCount { get; }
    }
}
