namespace ReactiveData.Sequence.IndexedList
{
    public interface IIndexedList<in TElement>
    {
        void Update(int index, int removeCount, TElement[] newItems);
    }
}