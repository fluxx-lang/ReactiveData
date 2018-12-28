using System;

namespace ReactiveData.Sequence
{
    public class ExpressionReactiveSequence<T> : ReactiveExpression<INonreactiveSequence<T>>, IReactiveSequence<T>
    {
        public ExpressionReactiveSequence(Func<INonreactiveSequence<T>> expressionFunction) : base(expressionFunction)
        {
        }
    }
}
