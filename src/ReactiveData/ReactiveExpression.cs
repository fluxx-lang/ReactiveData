using System;

namespace ReactiveData
{
    public class ReactiveExpression<TValue> : ReactiveChangableData<TValue>
    {
        private readonly Func<TValue> _expressionFunction;
        private TValue _value;
        private IReactive[] _dependencies = new IReactive[0];
        private int _staleCount = 0;


        public ReactiveExpression(Func<TValue> expressionFunction)
        {
            _expressionFunction = expressionFunction;
        }

        public override event ReactiveDataChangedEventHandler DataChanged {
            add {
                // If we're moving from lazy to reactive mode, because someone is now listening for changes, then compute our value
                // and update our dependencies, adding listeners for them
                if (!HaveSubscribers)
                    RecomputeDerivedValue();

                base.DataChanged += value;
            }

            remove {
                base.DataChanged -= value;

                // If we're moving from reactive mode to lazy mode, then forget the value (so it can be garbage collected) and stop
                // listening to our dependencies
                if (!HaveSubscribers) {
                    _value = default(TValue);

                    foreach (IReactive dependency in _dependencies)
                        dependency.DataChanged -= OnDependencyChanged;
                    _dependencies = new IReactive[0];
                }
            }
        }

        public override TValue Value {
            get {
                // If no one is listening for changes to us, we're in lazy mode so just evaluate the function on demand
                if (!HaveSubscribers)
                    return _expressionFunction.Invoke();

                RunningDerivation runningDerivation = RunningDerivationsStack.Top;
                if (runningDerivation == null)
                    throw new Exception("Can't get Value outside of a derivation; use CurrentVaue if don't want to register for changes");
                runningDerivation.AddDependency(this);

                return _value;
            }
        }

        public override TValue CurrentValue => !HaveSubscribers ? _expressionFunction.Invoke() : _value;

        private void OnDependencyChanged(State state)
        {
            if (state == State.Stale) {
                if (_staleCount == 0)
                    NotifyChanged(State.Stale);
                ++_staleCount;
            } else if (state == State.Ready) {
                --_staleCount;
                if (_staleCount == 0) {
                    RecomputeDerivedValue();
                    NotifyChanged(State.Ready);
                }
            }
        }

        private void RecomputeDerivedValue()
        {
            RunningDerivation runningDerivation = new RunningDerivation(_dependencies);
            RunningDerivation oldTopOfStack = RunningDerivationsStack.Top;
            RunningDerivationsStack.Top = runningDerivation;

            _value = _expressionFunction.Invoke();

            if (runningDerivation.DependenciesChanged)
                _dependencies = runningDerivation.UpdateDependencies(_dependencies, OnDependencyChanged);

            RunningDerivationsStack.Top = oldTopOfStack;
        }
    }
}
