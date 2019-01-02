namespace ReactiveData
{
    public interface IReactive
    {
        event ChangedEventHandler Changed;

        void AddExpressionDependingOnMe(IReactiveExpression reactiveExpression);

        void RemoveExpressionDependingOnMe(IReactiveExpression reactiveExpression);
    }

    public interface IReactive<out T> : IReactive
    {
        T Value { get; }
    }

    public delegate void ChangedEventHandler();
}