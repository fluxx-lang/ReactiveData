namespace ReactiveData
{
    /// <summary>
    /// 
    /// </summary>
    public interface IReactiveData
    {
        event DataChangedEventHandler DataChanged;

        void AddExpressionDependingOnMe(IReactiveExpression reactiveExpression);

        void RemoveExpressionDependingOnMe(IReactiveExpression reactiveExpression);
    }

    public interface IReactiveData<out T> : IReactiveData
    {
        T Value { get; }
    }

    public delegate void DataChangedEventHandler();
}