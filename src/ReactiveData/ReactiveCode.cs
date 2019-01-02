using System;

namespace ReactiveData
{
    public class ReactiveCode
    {
        private readonly Action _action;
        private IReactive[] _dependencies = new IReactive[0];


        public ReactiveCode(Action action) {
            _action = action;
            Run();
        }

        private void Run()
        {
            var runningDerivation = new RunningDerivation(_dependencies);
            RunningDerivation oldTopOfStack = RunningDerivationsStack.Top;
            RunningDerivationsStack.Top = runningDerivation;

            _action.Invoke();

            if (runningDerivation.DependenciesChanged)
                _dependencies = runningDerivation.UpdateReactionDependencies(_dependencies, Run);

            RunningDerivationsStack.Top = oldTopOfStack;
        }
    }
}