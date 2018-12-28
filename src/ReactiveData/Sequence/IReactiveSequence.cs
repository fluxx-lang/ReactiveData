namespace ReactiveData.Sequence
{
    public interface IReactiveSequence<T> : ISequence<T>, IReactive<INonreactiveSequence<T>>
    {
    }
}
