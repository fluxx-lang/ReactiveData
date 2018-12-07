namespace ReactiveData.ReactiveSequence
{
    public static class ListExtensions
    {
#if LATER
        public static TList WithReactiveItems<TList, TElment>(this TList list, ReactiveItemsBuilder<TElment> builder) where TList : IList<TElment>
        {
			new ReactiveItemsOnIList<TElment>(list, builder);
			return list;
        }
#endif
    }
}