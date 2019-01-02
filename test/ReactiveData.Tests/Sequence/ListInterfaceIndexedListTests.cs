using System.Collections.Generic;
using NUnit.Framework;
using ReactiveData.Sequence.IndexedList;

namespace ReactiveData.Tests.Sequence
{
    public class ListInterfaceIndexedListTests : TestsBase
    {
        [Test]
        public void TestInsert()
        {
            List<int> list = CreateTestList();
            var listInterfaceIndexedList = new ListInterfaceIndexedList<int>(list);

            listInterfaceIndexedList.Update(5, 0, new[] { 100, 101, 102 });
            AssertListRangeIs(list, start: 0, end: 5, firstValue: 0);
            AssertListRangeIs(list, start: 5, end: 8, firstValue: 100);
            AssertListRangeIs(list, start: 8, end: 13, firstValue: 5);
            Assert.AreEqual(13, list.Count);
        }

        [Test]
        public void TestReplace()
        {
            List<int> list = CreateTestList();
            var listInterfaceIndexedList = new ListInterfaceIndexedList<int>(list);

            listInterfaceIndexedList.Update(3, 5, new[] { 100, 101, 102, 103, 104 });
            AssertListRangeIs(list, start: 0, end: 3, firstValue: 0);
            AssertListRangeIs(list, start: 3, end: 8, firstValue: 100);
            AssertListRangeIs(list, start: 8, end: 10, firstValue: 8);
            Assert.AreEqual(10, list.Count);
        }

        [Test]
        public void TestReplaceAllWithLess()
        {
            List<int> list = CreateTestList();
            var listInterfaceIndexedList = new ListInterfaceIndexedList<int>(list);

            listInterfaceIndexedList.Update(0, 10, new[] { 100, 101, 102, 103, 104 });
            AssertListRangeIs(list, start: 0, end: 5, firstValue: 100);
            Assert.AreEqual(5, list.Count);
        }

        [Test]
        public void TestReplaceAllWithEqual()
        {
            List<int> list = CreateTestList();
            var listInterfaceIndexedList = new ListInterfaceIndexedList<int>(list);

            listInterfaceIndexedList.Update(0, 10, new[] { 100, 101, 102, 103, 104, 105, 106, 107, 108, 109 });
            AssertListRangeIs(list, start: 0, end: 10, firstValue: 100);
            Assert.AreEqual(10, list.Count);
        }

        [Test]
        public void TestReplaceAllWithMore()
        {
            List<int> list = CreateTestList();
            var listInterfaceIndexedList = new ListInterfaceIndexedList<int>(list);

            listInterfaceIndexedList.Update(0, 10, new[] { 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111 });
            AssertListRangeIs(list, start: 0, end: 12, firstValue: 100);
            Assert.AreEqual(12, list.Count);
        }

        [Test]
        public void TestReplaceAndRemove()
        {
            List<int> list = CreateTestList();
            var listInterfaceIndexedList = new ListInterfaceIndexedList<int>(list);

            listInterfaceIndexedList.Update(3, 7, new[] { 100, 101, 102, 103, 104 });
            AssertListRangeIs(list, start: 0, end: 3, firstValue: 0);
            AssertListRangeIs(list, start: 3, end: 8, firstValue: 100);
            Assert.AreEqual(8, list.Count);
        }

        [Test]
        public void TestReplaceAndInsert()
        {
            List<int> list = CreateTestList();
            var listInterfaceIndexedList = new ListInterfaceIndexedList<int>(list);

            listInterfaceIndexedList.Update(3, 2, new[] { 100, 101, 102, 103, 104 });
            AssertListRangeIs(list, start: 0, end: 3, firstValue: 0);
            AssertListRangeIs(list, start: 3, end: 8, firstValue: 100);
            AssertListRangeIs(list, start: 8, end: 13, firstValue: 5);
            Assert.AreEqual(13, list.Count);
        }

        [Test]
        public void TestReplaceAndAppend()
        {
            List<int> list = CreateTestList();
            var listInterfaceIndexedList = new ListInterfaceIndexedList<int>(list);

            listInterfaceIndexedList.Update(8, 2, new[] { 100, 101, 102, 103, 104 });
            AssertListRangeIs(list, start: 0, end: 8, firstValue: 0);
            AssertListRangeIs(list, start: 8, end: 13, firstValue: 100);
            Assert.AreEqual(13, list.Count);
        }

        private static List<int> CreateTestList()
        {
            var list = new List<int>();
            for (int i = 0; i < 10; ++i)
                list.Add(i);
            return list;
        }
    }
}