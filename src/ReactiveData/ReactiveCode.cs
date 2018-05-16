using System;

namespace ReactiveData
{
    public class ReactiveCode
    {
        private readonly Action _action;
        private IReactive[] _dependencies = new IReactive[0];
        private int _staleCount = 0;


        public ReactiveCode(Action action) {
            _action = action;
            Run();
        }

        private void OnDependencyChanged(State state)
        {
            if (state == State.Stale)
                ++_staleCount;
            else if (state == State.Ready)
            {
                --_staleCount;
                if (_staleCount == 0)
                    Run();
            }
        }

        private void Run()
        {
            RunningDerivation runningDerivation = new RunningDerivation(_dependencies);
            RunningDerivation oldTopOfStack = RunningDerivationsStack.Top;
            RunningDerivationsStack.Top = runningDerivation;

            _action.Invoke();

            if (runningDerivation.DependenciesChanged)
                _dependencies = runningDerivation.UpdateDependencies(_dependencies, OnDependencyChanged);

            RunningDerivationsStack.Top = oldTopOfStack;
        }
    }
}