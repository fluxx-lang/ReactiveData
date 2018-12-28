namespace ReactiveData.Sequence
{
    public sealed class ParentSequence<T> : IParentSequence<T>
    {
        private readonly ISequence<T>[] _children;

        public ParentSequence(ISequence<T>[] children)
        {
            _children = children;
        }

        public ISequence<T>[] Children => _children;

        public int ChildCount => _children.Length;
    }
}
