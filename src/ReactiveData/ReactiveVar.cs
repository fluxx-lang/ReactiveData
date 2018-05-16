using System;
using System.Collections.Generic;

namespace ReactiveData {
    /// <summary>
    /// A ReactiveVar can be changed by calling set on the Value, in which case the change notification is sent,
    /// or by mutating the Value's existing content and then calling NotifyChanged.
    /// </summary>
    /// <typeparam name="TValue">type of the value to wrap</typeparam>
    public sealed class ReactiveVar<TValue> : ReactiveChangableData<TValue> {
        private TValue _value;

	    public ReactiveVar(TValue value)
	    {
	        _value = value;
	    }

        public void UpdateValue(TValue value)
        {
            NotifyChanged(State.Stale);
            _value = value;
            NotifyChanged(State.Ready);
        }

        public override TValue Value {
            get {
                RunningDerivation runningDerivation = RunningDerivationsStack.Top;
                if (runningDerivation == null)
                    throw new Exception("Can't get Value outside of a derivation; use CurrentVaue if don't want to register for changes");
                runningDerivation.AddDependency(this);

                return _value;
            }
        }

        public override TValue CurrentValue => _value;
    }
}
