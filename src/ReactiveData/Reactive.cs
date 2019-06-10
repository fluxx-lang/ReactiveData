namespace ReactiveData {
    /// <summary>
    /// A Reactive object has data of type TValue, essentially turning a "normal" type TValue into a "reactive" one,
    /// where one can be notified of changes, use the object in reactive expressions that are automatically
    /// recomputed on change, etc.
    /// </summary>
    /// <typeparam name="TValue">data type of value</typeparam>
    public abstract class Reactive<TValue> : ReactiveBase
    {
        public abstract TValue Value { get; }
    }
}
