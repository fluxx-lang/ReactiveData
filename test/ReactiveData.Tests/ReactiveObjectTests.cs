using System;
using NUnit.Framework;

namespace ReactiveData.Tests
{
    public class ReactiveObjectTests : TestsBase
    {
        [Test]
        public void TestEnsureInTransaction()
        {
            var testObject = new TestObject();

            // Ensure that we only allow changes inside a transaction
            Assert.That(() => testObject.IntValue = 42, Throws.Exception.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void TestNotifyChanged()
        {
            var testObject = new TestObject();
            ChangedCalled changedCalled = EnsureChangedCalled(testObject);

            Transaction.Start();
            testObject.IntValue = 1;
            testObject.IntValue = 2;
            testObject.IntValue = 3;
            CompleteTransactionAndAssertChangedCalled(changedCalled);
        }

        private class TestObject : ReactiveObject
        {
            private int _intValue;

            public int IntValue
            {
                get => Get(_intValue);
                set => Set(out _intValue, value);
            }
        }
    }
}