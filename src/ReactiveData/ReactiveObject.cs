using System;
using System.Collections.Generic;

#if false

namespace ReactiveData {
    /// <summary>
    /// A ReactiveVar can be changed by calling set on the Value, in which case the change notification is sent,
    /// or by mutating the Value's existing content and then calling NotifyChanged.
    /// </summary>
    /// <typeparam name="TObject">type of the value to wrap</typeparam>
    public sealed class ReactiveObject<TObject> : ReactiveChangableData<TObject> {
        private TObject _obj;

	    public ReactiveObject(TObject obj)
	    {
	        _obj = obj;
	    }

        public TProperty Get<TProperty>(Func<TProperty> getProperty, string propertyName)
        {
            return getProperty();
        }

        public override TObject CurrentValue => _obj;
    }
}

#endif