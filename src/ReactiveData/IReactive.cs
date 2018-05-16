namespace ReactiveData
{
    public interface IReactive
	{
	    event ReactiveDataChangedEventHandler DataChanged;
    }

    public interface IReactive<out T> : IReactive
    {
        T Value { get; }
    }

    public enum State
    {
        Stale, Ready
    }

    public delegate void ReactiveDataChangedEventHandler(State state);
}