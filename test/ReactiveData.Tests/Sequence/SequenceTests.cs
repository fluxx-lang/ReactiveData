using System.Collections.Generic;
using NUnit.Framework;
using ReactiveData.Sequence;
using ReactiveData.Sequence.IndexedList;
using static ReactiveData.Sequence.SequenceUtils;

namespace ReactiveData.Tests.Sequence
{
    public class SequenceTests : TestsBase
    {
        [Test]
        public void TestParentSequence()
        {
            ParentSequence<int> parentSequence = Sequences(
                Items(0, 1, 2),
                Sequences(
                    Items(3, 4, 5),
                    Items(6, 7, 8, 9)
                )
            );

            Assert.AreEqual(2, parentSequence.ChildCount);

            ISequence<int> lastChild = parentSequence.Children[1];
            ISequence<int> lastGrandchild = ((IParentSequence<int>) lastChild).Children[1];
            int lastItem = ((IItemsSequence<int>) lastGrandchild).Items[3];

            Assert.AreEqual(9, lastItem);
        }

        [Test]
        public void TestIndexedListOnSequence()
        {
            ParentSequence<int> rootSequence = Sequences(
                Items(0, 1, 2),
                Sequences(
                    Items(3, 4, 5),
                    Items(6, 7, 8, 9)
                )
            );

            var list = new List<int>();
            new IndexedListOnSequence<int>(new ListInterfaceIndexedList<int>(list), rootSequence);

            Assert.AreEqual(10, list.Count);
            AssertListRangeIs(list, start: 0, end: 10, firstValue: 0);
        }

        [Test]
        public void TestIndexedListOnReactiveSequence()
        {
            var length = new ReactiveVar<int>(2);

            ParentSequence<int> rootSequence = Sequences(
                Items(0, 1, 2),
                Sequences(
                    Expression(() => CreateTestSequenceOfLength(length.Value)),
                    Items(3, 4, 5)
                )
            );

            var list = new List<int>();
            new IndexedListOnSequence<int>(new ListInterfaceIndexedList<int>(list), rootSequence);

            AssertListRangeIs(list, start: 0, end: 3, firstValue: 0);
            AssertListRangeIs(list, start: 3, end: 5, firstValue: 100);
            AssertListRangeIs(list, start: 5, end: 8, firstValue: 3);
            Assert.AreEqual(8, list.Count);

            Transaction.Run(() => length.Set(3));
            AssertListRangeIs(list, start: 0, end: 3, firstValue: 0);
            AssertListRangeIs(list, start: 3, end: 6, firstValue: 100);
            AssertListRangeIs(list, start: 6, end: 9, firstValue: 3);
            Assert.AreEqual(9, list.Count);

            Transaction.Run(() => length.Set(0));
            AssertListRangeIs(list, start: 0, end: 3, firstValue: 0);
            AssertListRangeIs(list, start: 3, end: 6, firstValue: 3);
            Assert.AreEqual(6, list.Count);
        }

        private static IItemsSequence<int> CreateTestSequenceOfLength(int length)
        {
            var list = new List<int>();
            for (int i = 0; i < length; ++i)
                list.Add(100 + i);

            return Items(list.ToArray());
        }
    }
}