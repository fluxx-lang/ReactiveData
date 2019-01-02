using System.Collections.Generic;
using NUnit.Framework;

namespace ReactiveData.Tests
{
    public class TestsBase
    {
        [SetUp]
        public void Setup()
        {
        }

        protected class ChangedCalled
        {
            private int _calledCount;

            public void Increment()
            {
                ++_calledCount;
            }

            public void Reset()
            {
                _calledCount = 0;
            }

            public void AssertNotCalled()
            {
                Assert.IsTrue(_calledCount == 0, "Changed called when not expected");
            }

            public void AssertCalled()
            {
                Assert.AreEqual(1, _calledCount);
            }
        }

        protected ChangedCalled EnsureChangedCalled<T>(IReactive<T> reactive)
        {
            var changedCalled = new ChangedCalled();

            reactive.Changed += () => changedCalled.Increment();

            return changedCalled;
        }

        protected static void CompleteTransactionAndAssertChangedCalled(ChangedCalled changedCalled)
        {
            changedCalled.AssertNotCalled();
            Transaction.Complete();
            changedCalled.AssertCalled();
        }

        protected static void AssertListRangeIs(List<int> list, int start, int end, int firstValue)
        {
            int count = end - start;
            for (int i = 0; i < count; ++i)
                Assert.AreEqual(firstValue + i, list[start + i], $"Item at list position {start + i} has unexpected value");
        }
    }
}