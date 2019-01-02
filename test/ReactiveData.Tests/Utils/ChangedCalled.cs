using NUnit.Framework;

namespace ReactiveData.Tests.Utils
{
    class ChangedCalled
    {
        public bool Called { private get; set; }

        public void AssertNotCalled()
        {
            Assert.IsFalse(Called, "Changed called when not expected");
        }

        public void AssertCalled()
        {
            Assert.IsTrue(Called, "Changed never called");
        }
    }
}