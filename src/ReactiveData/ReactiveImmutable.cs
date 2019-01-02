namespace ReactiveData
{
    public class ReactiveImmutable<T> : IReactive<T>
    {
        private readonly T _value;

        public ReactiveImmutable(T value)
        {
            _value = value;
        }

        /// <summary>
        /// Since data never changes for a ReactiveImmutable, don't store any subscribers.
        /// </summary>
        public event ChangedEventHandler Changed {
            add { }
            remove { }
        }

        public void AddExpressionDependingOnMe(IReactiveExpression reactiveExpression)
        {
        }

        public void RemoveExpressionDependingOnMe(IReactiveExpression reactiveExpression)
        {
        }

        public T Value => _value;
    }
}
