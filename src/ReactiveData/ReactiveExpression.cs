using System;
using System.ComponentModel;

namespace ReactiveData
{
    public interface IReactiveExpression
    {
        void OnDependencyChanged();
    }

    public class ReactiveExpression<TValue> : Reactive<TValue>, IReactiveExpression
    {
        private readonly Func<TValue> _expressionFunction;
        private TValue _value;
        private IReactive[] _dependencies = new IReactive[0];

        public ReactiveExpression(Func<TValue> expressionFunction)
        {
            _expressionFunction = expressionFunction;
        }

        public override event ChangedEventHandler Changed {
            add {
                // If we're moving from lazy to reactive mode, because someone is now listening for changes, then compute our value
                // and update our dependencies, adding listeners for them
                if (!HaveSubscribers)
                    RecomputeDerivedValue();

                base.Changed += value;
            }

            remove {
                base.Changed -= value;

                // If we're moving from reactive mode to lazy mode, then forget the value (so it can be garbage collected) and stop
                // listening to our dependencies
                if (!HaveSubscribers) {
                    _value = default(TValue);

                    foreach (IReactive dependency in _dependencies)
                        dependency.RemoveExpressionDependingOnMe(this);
                    _dependencies = new IReactive[0];
                }
            }
        }

        public override TValue Value {
            get {
                // If no one is listening for changes to us, we're in lazy mode so just evaluate the function on demand
                if (!HaveSubscribers)
                    return _expressionFunction.Invoke();

                RunningDerivationsStack.Top?.AddDependency(this);

                return _value;
            }
        }

        public void OnDependencyChanged()
        {
            TValue oldValue = _value;

            RecomputeDerivedValue();

            // If not changing, don't notify
            if (_value.Equals(oldValue))
                return;

            NotifyChanged();
        }

        private void RecomputeDerivedValue()
        {
            var runningDerivation = new RunningDerivation(_dependencies);
            RunningDerivation oldTopOfStack = RunningDerivationsStack.Top;
            RunningDerivationsStack.Top = runningDerivation;

            _value = _expressionFunction.Invoke();

            if (runningDerivation.DependenciesChanged)
                _dependencies = runningDerivation.UpdateExpressionDependencies(_dependencies, this);

            RunningDerivationsStack.Top = oldTopOfStack;
        }
    }
}
