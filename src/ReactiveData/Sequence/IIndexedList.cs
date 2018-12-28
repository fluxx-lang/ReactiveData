namespace ReactiveData.ReactiveSequences
{
    public interface IIndexedList<in TElement>
    {
        void Replace(int index, int removeCount, TElement[] newElements);
    }
}