using NUnit.Framework;

namespace ReactiveData.Tests
{
    public class ReactiveVarTests : TestsBase
    {
        [Test]
        public void TestSet()
        {
            var reactiveVar = new ReactiveVar<int>(1);
            ChangedCalled changedCalled = EnsureChangedCalled(reactiveVar);

            Transaction.Start();
            reactiveVar.Set(2);
            changedCalled.AssertNotCalled();

            Transaction.Complete();
            changedCalled.AssertCalled();

            Assert.AreEqual(2, reactiveVar.Value);
        }

        [Test]
        public void TestNotifyChanged()
        {
            var reactiveVar = new ReactiveVar<int>(1);
            ChangedCalled changedCalled = EnsureChangedCalled(reactiveVar);

            Transaction.Start();
            reactiveVar.NotifyChanged();
            changedCalled.AssertNotCalled();

            Transaction.Complete();
            changedCalled.AssertCalled();

            Assert.AreEqual(1, reactiveVar.Value);
        }
    }
}