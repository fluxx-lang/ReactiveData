using System.Collections.Generic;

namespace ReactiveData.ReactiveSequence
{
    public class ReactiveItemsOnIList<TElement> : ReactiveItemsOn<TElement>
    {
        private readonly IList<TElement> _list;

		public ReactiveItemsOnIList(IList<TElement> list, ReactiveItemsBuilder<TElement> itemsBuilder) : base(itemsBuilder)
        {
            _list = list;
	        Init();
        }

	    public override void Replace(int index, int countToReplace, IEnumerable<TElement> newItems)
	    {
		    for (int i = 0; i < countToReplace; i++)
			    _list.RemoveAt(index);

		    int j = index;
		    foreach (TElement newItem in newItems)
		    {
			    if (j < _list.Count)
				    _list[j] = newItem;
			    else _list.Add(newItem);
			    ++j;
			}
	    }
	    
    }
}