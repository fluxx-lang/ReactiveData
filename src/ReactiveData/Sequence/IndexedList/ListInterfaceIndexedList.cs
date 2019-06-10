using System;
using System.Collections.Generic;

namespace ReactiveData.Sequence.IndexedList
{
    public class ListInterfaceIndexedList<T> : IIndexedList<T>
    {
        private readonly IList<T> _list;

        public ListInterfaceIndexedList(IList<T> list)
        {
            _list = list;
        }

        public void Update(int index, int removeCount, T[] newItems)
        {
            int listCount = _list.Count;
            if (index < 0 || index > listCount)
                throw new ArgumentException($"Index {index} is out of range for list of length {listCount}");
            if (removeCount > listCount - index)
                throw new ArgumentException($"Can't remove {removeCount} items when only {listCount - index} items are left in the list");

            var newItemsCount = newItems.Length;

            if (removeCount == listCount) {
                _list.Clear();
                removeCount = 0;
            }

            // Update in place where we can
            int updateCount = Math.Min(removeCount, newItemsCount);
            for (int i = 0; i < updateCount; ++i)
                _list[index + i] = newItems[i];

            // If there are remaining new items, insert/append them
            if (newItemsCount > updateCount) {
                for (int i = updateCount; i < newItemsCount; ++i)
                    _list.Insert(index + i, newItems[i]);
            }
            // If there are remaining items to remove, remove them
            else {
                int removeStartIndex = index + updateCount;

                for (int i = index + removeCount - 1; i >= removeStartIndex; --i)
                    _list.RemoveAt(i);
            }
        }
    }
}