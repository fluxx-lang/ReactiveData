namespace ReactiveData.Sequence
{
    public interface IParentSequence<T> : INonreactiveSequence<T>
    {
        ISequence<T>[] Children { get; }

        int ChildCount { get; }
    }
}
