using System.Collections.Generic;
using System.ComponentModel;
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

        protected class PropertyChangedCalled
        {
            private object? _sender;
            private PropertyChangedEventArgs? _args;

            public void Set(object sender, PropertyChangedEventArgs args)
            {
                if (_sender != null || _args != null)
                    throw new AssertionException("PropertyChanged called multiple times, when not expected");

                _sender = sender;
                _args = args;
            }

            public void AssertNotCalled()
            {
                Assert.IsTrue(_sender == null && _args == null, "PropertyChanged called when not expected");
            }

            public void AssertCalledWith(object expectedSender, string expectedPropertyName)
            {
                Assert.AreEqual(expectedSender, _sender);
                Assert.AreEqual(expectedPropertyName, _args.PropertyName);
            }

            public void Reset()
            {
                _sender = null;
                _args = null;
            }
        }

        protected ChangedCalled EnsureChangedCalled(IReactive reactive)
        {
            var changedCalled = new ChangedCalled();

            reactive.Changed += () => changedCalled.Increment();

            return changedCalled;
        }

        protected PropertyChangedCalled EnsurePropertyChangedCalled(INotifyPropertyChanged inpcObject)
        {
            var propertyChangedCalled = new PropertyChangedCalled();

            inpcObject.PropertyChanged += (sender, args) => propertyChangedCalled.Set(sender, args);

            return propertyChangedCalled;
        }

        protected static void CompleteTransactionAndAssertChangedCalled(ChangedCalled changedCalled)
        {
            changedCalled.AssertNotCalled();
            Transaction.End();
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