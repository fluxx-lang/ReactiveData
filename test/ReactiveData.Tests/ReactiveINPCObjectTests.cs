using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace ReactiveData.Tests
{
    public class ReactiveINPCObjectTests : TestsBase
    {
        [Test]
        public void TestEnsureInTransaction()
        {
            var testObject = new TestINPCObject();
            var reactiveTestObject = new ReactiveINPCObject<TestINPCObject>(testObject);

            // Ensure that we only allow changes inside a transaction
            Assert.That(() => testObject.TestProperty = 2, Throws.Exception.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void TestNotifyChanged()
        {
            var testObject = new TestINPCObject();
            var reactiveTestObject = new ReactiveINPCObject<TestINPCObject>(testObject);
            ChangedCalled changedCalled = EnsureChangedCalled(reactiveTestObject);

            Transaction.Start();
            testObject.TestProperty = 2;
            testObject.TestProperty = 3;
            testObject.TestProperty = 4;
            CompleteTransactionAndAssertChangedCalled(changedCalled);
        }

        private class TestINPCObject : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            private int _testProperty;

            public int TestProperty {
                get => _testProperty;
                set {
                    if (value != _testProperty) {
                        _testProperty = value;
                        OnPropertyChanged();
                    }
                }
            }

            private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}