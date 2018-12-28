using System.Collections.Generic;

namespace ReactiveData {
    public abstract class ReactiveMutable<TValue> : IReactive<TValue>
    {
        private List<IReactiveExpression> _expressionsDependingOnMe;

        public virtual event DataChangedEventHandler DataChanged;

        public void NotifyChanged()
        {
            // Immediately notify any expressions depending on me, traversing the graph upwards
            if (_expressionsDependingOnMe != null) {
                var count = _expressionsDependingOnMe.Count;
                for (int i = 0; i < count; i++)
                    _expressionsDependingOnMe[i].OnDependencyChanged();
            }

            // If anyone external wants to be notified of changes, record that and the notification happens
            // when the transaction completes
            if (DataChanged != null)
                Transaction.AddToNotify(DataChanged);
        }

        protected bool HaveSubscribers => DataChanged != null || _expressionsDependingOnMe?.Count > 0;

        public void AddExpressionDependingOnMe(IReactiveExpression reactiveExpression)
        {
            if (_expressionsDependingOnMe == null)
                _expressionsDependingOnMe = new List<IReactiveExpression>();
            _expressionsDependingOnMe.Add(reactiveExpression);
        }

        public void RemoveExpressionDependingOnMe(IReactiveExpression reactiveExpression)
        {
            _expressionsDependingOnMe?.Remove(reactiveExpression);
        }

        public abstract TValue Value { get; }
    }
}
