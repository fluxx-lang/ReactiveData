using System;
using System.Collections.Generic;
using System.Linq;

namespace ReactiveData.ReactiveSequence
{
    public class ReactiveOptionalItems<TElement> : ReactiveExpression<IEnumerable<TElement>>
    {
        public ReactiveOptionalItems(Func<bool> when, params TElement[] items) :
	        base(() => {
		        bool includeItems = when.Invoke();
		        return includeItems ? items : Enumerable.Empty<TElement>();
	        })
        {
        }
    }
}