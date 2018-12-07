namespace ReactiveData {
    public class ReactiveConstant<TValue> : IReactiveData<TValue> {
        private readonly TValue _value;

        public ReactiveConstant(TValue value) {
            _value = value;
        }

        /// <summary>
        /// Since data never changes for a ReactiveConstant, don't store any subscribers.
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
