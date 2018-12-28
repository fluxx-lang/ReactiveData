namespace ReactiveData {
    public class ReactiveValue<TValue> : IReactive<TValue> {
        private readonly TValue _value;

        public ReactiveValue(TValue value) {
            _value = value;
        }

        /// <summary>
        /// Since data never changes for a ReactiveValue, don't store any subscribers.
        /// </summary>
        public event DataChangedEventHandler DataChanged {
            add { }
            remove { }
        }

        public void AddExpressionDependingOnMe(IReactiveExpression reactiveExpression)
        {
        }

        public void RemoveExpressionDependingOnMe(IReactiveExpression reactiveExpression)
        {
        }

        public TValue Value => _value;
    }
}
