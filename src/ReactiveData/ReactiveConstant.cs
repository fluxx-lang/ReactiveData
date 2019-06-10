namespace ReactiveData
{
    /// <summary>
    /// A ReactiveConstant is like a ReactiveVar except that it never changes and thus has no Set
    /// method. It can be used when there's something that doesn't change but still needs to be
    /// treated as type Reactive, perhaps to pass to some method that handles data that sometimes
    /// changes.
    /// </summary>
    /// <typeparam name="TValue">value data type</typeparam>
    public class ReactiveConstant<TValue> : Reactive<TValue>
    {
        private readonly TValue _value;

        public ReactiveConstant(TValue value)
        {
            _value = value;
        }

        /// <summary>
        /// Since data never changes for a ReactiveConstant, don't store any subscribers.
        /// </summary>
        public override event ChangedEventHandler Changed {
            add { }
            remove { }
        }

        public override TValue Value => _value;
    }
}
