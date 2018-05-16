namespace ReactiveData {
    public class ReactiveConstant<TValue> : IReactive<TValue> {
        private readonly TValue _value;


        public ReactiveConstant(TValue value) {
            _value = value;
        }

        /// <summary>
        /// Since data never changes for a ReactiveConstant, don't store any subscribers.
        /// </summary>
        public event ReactiveDataChangedEventHandler ReactiveDataChanged {
            add { }
            remove { }
        }

        public TValue Value => _value;
    }
}
