namespace ReactiveData.Sequence
{
    public interface IParentSequence<T> : INonreactiveSequence<T>
    {
        SequenceImmutableArray<ISequence<T>> Children { get; }

        int ChildCount { get; }
    }
}
