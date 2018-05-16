using System.Collections.Generic;

namespace ReactiveData.ReactiveSequence
{
    public static class ListExtensions
    {
        public static TList WithReactiveItems<TList, TElment>(this TList list, ReactiveItemsBuilder<TElment> builder) where TList : IList<TElment>
        {
			new ReactiveItemsOnIList<TElment>(list, builder);
			return list;
        }
    }
}