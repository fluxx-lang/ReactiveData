namespace ReactiveData.Sequence
{
    public sealed class ParentSequence<T> : IParentSequence<T>
    {
        private readonly SequenceImmutableArray<ISequence<T>> _children;

        public ParentSequence(SequenceImmutableArray<ISequence<T>> children)
        {
            _children = children;
        }

        public SequenceImmutableArray<ISequence<T>> Children => _children;

        public int ChildCount => _children.Count;
    }
}
