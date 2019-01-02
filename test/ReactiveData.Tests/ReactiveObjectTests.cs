using System;
using NUnit.Framework;

namespace ReactiveData.Tests
{
    public class ReactiveObjectTests : TestsBase
    {
        [Test]
        public void TestEnsureInTransaction()
        {
            TestObject testObject = new TestObject();
            var reactiveTestObject = new ReactiveObject<TestObject>(testObject);

            // Ensure that we only allow changes inside a transaction
            Assert.That(() => testObject.SetSomething(), Throws.Exception.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void TestNotifyChanged()
        {
            TestObject testObject = new TestObject();
            var reactiveTestObject = new ReactiveObject<TestObject>(testObject);
            ChangedCalled changedCalled = EnsureChangedCalled(reactiveTestObject);

            Transaction.Start();
            testObject.SetSomething();
            testObject.SetSomething();
            testObject.SetSomething();
            CompleteTransactionAndAssertChangedCalled(changedCalled);
        }

        private class TestObject : INotifyObjectChanged
        {
            public event ObjectChangedEventHandler ObjectChanged;

            public void SetSomething()
            {
                ObjectChanged?.Invoke();
            }
        }
    }
}