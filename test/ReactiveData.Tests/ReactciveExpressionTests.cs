using NUnit.Framework;

namespace ReactiveData.Tests
{
    public class ReactiveExpressionTests : TestsBase
    {
        [Test]
        public void TestSimpleExpression()
        {
            var term1 = new ReactiveVar<int>(1);
            var term2 = new ReactiveVar<int>(2);

            var expression = new ReactiveExpression<int>(() => term1.Value + term2.Value);
            Assert.AreEqual(3, expression.Value);

            ChangedCalled changedCalled = EnsureChangedCalled(expression);
            Transaction.Start();
            term1.Set(3);
            term2.Set(4);
            CompleteTransactionAndAssertChangedCalled(changedCalled);
            Assert.AreEqual(7, expression.Value);

            changedCalled = EnsureChangedCalled(expression);
            Transaction.Start();
            term1.Set(5);
            CompleteTransactionAndAssertChangedCalled(changedCalled);
            Assert.AreEqual(9, expression.Value);
        }

        [Test]
        public void TestNestedExpressions()
        {
            var term1 = new ReactiveVar<int>(1);
            var term2 = new ReactiveVar<int>(2);

            var subexpression = new ReactiveExpression<int>(() => term1.Value + term2.Value);
            ChangedCalled subexpressionChangedCalled = EnsureChangedCalled(subexpression);

            var term3 = new ReactiveVar<int>(3);
            var expression = new ReactiveExpression<int>(() => subexpression.Value + term3.Value);
            ChangedCalled expressionChangeCalled = EnsureChangedCalled(expression);

            Transaction.Start();
            term3.Set(5);
            CompleteTransactionAndAssertChangedCalled(expressionChangeCalled);
            Assert.AreEqual(8, expression.Value);

            expressionChangeCalled.Reset();
            Transaction.Start();
            term1.Set(10);
            CompleteTransactionAndAssertChangedCalled(expressionChangeCalled);
            subexpressionChangedCalled.AssertCalled();

            Assert.AreEqual(17, expression.Value);
        }
    }
}