using System;
using NUnit.Framework;

namespace ReactiveData.Tests
{
    public class ReactiveInpcObjectTests : TestsBase
    {
        [Test]
        public void TestEnsureInTransaction()
        {
            var testObject = new TestInpcObject();

            // Ensure that we only allow changes inside a transaction
            Assert.That(() => testObject.IntValue = 42, Throws.Exception.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void TestNotifyChanged()
        {
            var testObject = new TestInpcObject();
            ChangedCalled changedCalled = EnsureChangedCalled(testObject);
            PropertyChangedCalled propertyChangedCalled = EnsurePropertyChangedCalled(testObject);

            Transaction.Start();

            testObject.IntValue = 1;
            propertyChangedCalled.AssertCalledWith(testObject, "IntValue");
            propertyChangedCalled.Reset();

            testObject.IntValue = 2;
            propertyChangedCalled.AssertCalledWith(testObject, "IntValue");
            propertyChangedCalled.Reset();

            testObject.IntValue = 3;
            propertyChangedCalled.AssertCalledWith(testObject, "IntValue");

            CompleteTransactionAndAssertChangedCalled(changedCalled);
        }

        private class TestInpcObject : ReactiveInpcObject
        {
            private int _intValue;

            public int IntValue
            {
                get => Get(_intValue);
                set => Set(ref _intValue, value);
            }
        }
    }
}