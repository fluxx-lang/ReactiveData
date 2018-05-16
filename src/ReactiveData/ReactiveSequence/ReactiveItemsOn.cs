using System.Collections.Generic;
using System.Linq;
using ReactiveData.ReactiveSequence;

namespace ReactiveData.ReactiveSequence
{
    public abstract class ReactiveItemsOn<TElement>
    {
        private readonly List<IReactive<IEnumerable<TElement>>> _subsequences;
	    private readonly List<CachedEnumerable> _currSubsequenceValues;

	    protected ReactiveItemsOn(ReactiveItemsBuilder<TElement> itemsBuilder)
        {
	        _subsequences = itemsBuilder.GetSubsequences();
	        _currSubsequenceValues = new List<CachedEnumerable>();
        }

	    public abstract void Replace(int index, int countToReplace, IEnumerable<TElement> newItems);

	    protected void Init()
	    {
	        new ReactiveCode(Update);
	    }

	    private void Update()
	    {
		    int currentOffset = 0;
		    for (int i = 0; i < _subsequences.Count; i++)
		    {
			    IReactive<IEnumerable<TElement>> newSubsequence = _subsequences[i];
			    CachedEnumerable newSubsequenceValue = new CachedEnumerable(newSubsequence.Value);

                CachedEnumerable currSubsequenceValue = i < _currSubsequenceValues.Count ? _currSubsequenceValues[i] : null;

			    if (currSubsequenceValue != null && currSubsequenceValue.HasSameEnumerableAs(newSubsequenceValue))
				    currentOffset += currSubsequenceValue.Count;
			    else
			    {
				    int countToRemove = currSubsequenceValue != null ? currSubsequenceValue.Count : 0;
					Replace(currentOffset, countToRemove, newSubsequenceValue.List);
			        currentOffset += newSubsequenceValue.Count - countToRemove;

				    if (i < _currSubsequenceValues.Count)
					    _currSubsequenceValues[i] = newSubsequenceValue;
				    else _currSubsequenceValues.Add(newSubsequenceValue);
			    }
			}
		}

        private class CachedEnumerable
        {
            private readonly IEnumerable<TElement> _enumerable;
            private readonly IList<TElement> _list;

            public CachedEnumerable(IEnumerable<TElement> enumerable)
            {
                _enumerable = enumerable;
                _list = enumerable.ToList();
            }

            public IList<TElement> List => _list;

            public int Count => _list.Count;

            public bool HasSameEnumerableAs(CachedEnumerable other) => ReferenceEquals(other._enumerable, _enumerable);
        }
    }
}