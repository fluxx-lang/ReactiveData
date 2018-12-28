using System;
using System.Collections.Generic;
using System.Linq;

namespace ReactiveData {
    internal class RunningDerivation {
        private readonly IReactive[] _currentDependencies;
        private int _nextDependencyIndex = 0;
        private List<IReactive> _newDependencies = null;


        public RunningDerivation(IReactive[] currentDependencies) {
            _currentDependencies = currentDependencies;
        }

        public void AddDependency(IReactive reactive)
        {
            if (_nextDependencyIndex < _currentDependencies.Length && _currentDependencies[_nextDependencyIndex] == reactive)
            {
                ++_nextDependencyIndex;
                return;
            }

            if (_newDependencies == null)
            {
                _newDependencies = new List<IReactive>();
                for (int i = 0; i < _nextDependencyIndex; ++i)
                    _newDependencies.Add(_currentDependencies[i]);

                _nextDependencyIndex = _currentDependencies.Length + 1;
            }

            _newDependencies.Add(reactive);
        }

        public bool DependenciesChanged => _nextDependencyIndex != _currentDependencies.Length;

        internal IReactive[] UpdateExpressionDependencies(IReactive[] oldDependencies, IReactiveExpression reactiveExpression)
        {
            if (_nextDependencyIndex < _currentDependencies.Length) 
            {
                if (_newDependencies != null)
                    throw new Exception("_newDependencies unexpectedly initialized when _nextDependencyIndex is before the end");

                _newDependencies = new List<IReactive>();
                for (int i = 0; i < _nextDependencyIndex; ++i)
                    _newDependencies.Add(_currentDependencies[i]);
            }

            IReactive[] newDependenciesArray = _newDependencies.ToArray();

            var currDependenciesSet = new HashSet<IReactive>();
            foreach (IReactive currDependency in oldDependencies)
                currDependenciesSet.Add(currDependency);

            var newDependenciesSet = new HashSet<IReactive>();
            foreach (IReactive newDependency in newDependenciesArray)
                newDependenciesSet.Add(newDependency);

            // TODO: Catch exceptions here to ensure in consistent state
            foreach (IReactive removeDependency in currDependenciesSet.Except(newDependenciesSet))
                removeDependency.RemoveExpressionDependingOnMe(reactiveExpression);

            foreach (IReactive addDependency in newDependenciesArray.Except(currDependenciesSet))
                addDependency.AddExpressionDependingOnMe(reactiveExpression);

            return newDependenciesArray;
        }

        internal IReactive[] UpdateCodeDependencies(IReactive[] oldDependencies, DataChangedEventHandler dataChanged)
        {
            if (_nextDependencyIndex < _currentDependencies.Length) {
                if (_newDependencies != null)
                    throw new Exception("_newDependencies unexpectedly initialized when _nextDependencyIndex is before the end");

                _newDependencies = new List<IReactive>();
                for (int i = 0; i < _nextDependencyIndex; ++i)
                    _newDependencies.Add(_currentDependencies[i]);
            }

            IReactive[] newDependenciesArray = _newDependencies.ToArray();

            var currDependenciesSet = new HashSet<IReactive>();
            foreach (IReactive currDependency in oldDependencies)
                currDependenciesSet.Add(currDependency);

            var newDependenciesSet = new HashSet<IReactive>();
            foreach (IReactive newDependency in newDependenciesArray)
                newDependenciesSet.Add(newDependency);

            // TODO: Catch exceptions here to ensure in consistent state
            foreach (IReactive removeDependency in currDependenciesSet.Except(newDependenciesSet))
                removeDependency.DataChanged -= dataChanged;

            foreach (IReactive addDependency in newDependenciesArray.Except(currDependenciesSet))
                addDependency.DataChanged += dataChanged;

            return newDependenciesArray;
        }
    }
}
