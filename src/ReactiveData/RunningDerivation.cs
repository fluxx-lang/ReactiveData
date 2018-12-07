using System;
using System.Collections.Generic;
using System.Linq;

namespace ReactiveData {
    internal class RunningDerivation {
        private readonly IReactiveData[] _currentDependencies;
        private int _nextDependencyIndex = 0;
        private List<IReactiveData> _newDependencies = null;


        public RunningDerivation(IReactiveData[] currentDependencies) {
            _currentDependencies = currentDependencies;
        }

        public void AddDependency(IReactiveData reactiveData)
        {
            if (_nextDependencyIndex < _currentDependencies.Length && _currentDependencies[_nextDependencyIndex] == reactiveData)
            {
                ++_nextDependencyIndex;
                return;
            }

            if (_newDependencies == null)
            {
                _newDependencies = new List<IReactiveData>();
                for (int i = 0; i < _nextDependencyIndex; ++i)
                    _newDependencies.Add(_currentDependencies[i]);

                _nextDependencyIndex = _currentDependencies.Length + 1;
            }

            _newDependencies.Add(reactiveData);
        }

        public bool DependenciesChanged => _nextDependencyIndex != _currentDependencies.Length;

        internal IReactiveData[] UpdateExpressionDependencies(IReactiveData[] oldDependencies, IReactiveExpression reactiveExpression)
        {
            if (_nextDependencyIndex < _currentDependencies.Length) 
            {
                if (_newDependencies != null)
                    throw new Exception("_newDependencies unexpectedly initialized when _nextDependencyIndex is before the end");

                _newDependencies = new List<IReactiveData>();
                for (int i = 0; i < _nextDependencyIndex; ++i)
                    _newDependencies.Add(_currentDependencies[i]);
            }

            IReactiveData[] newDependenciesArray = _newDependencies.ToArray();

            var currDependenciesSet = new HashSet<IReactiveData>();
            foreach (IReactiveData currDependency in oldDependencies)
                currDependenciesSet.Add(currDependency);

            var newDependenciesSet = new HashSet<IReactiveData>();
            foreach (IReactiveData newDependency in newDependenciesArray)
                newDependenciesSet.Add(newDependency);

            // TODO: Catch exceptions here to ensure in consistent state
            foreach (IReactiveData removeDependency in currDependenciesSet.Except(newDependenciesSet))
                removeDependency.RemoveExpressionDependingOnMe(reactiveExpression);

            foreach (IReactiveData addDependency in newDependenciesArray.Except(currDependenciesSet))
                addDependency.AddExpressionDependingOnMe(reactiveExpression);

            return newDependenciesArray;
        }

        internal IReactiveData[] UpdateCodeDependencies(IReactiveData[] oldDependencies, DataChangedEventHandler dataChanged)
        {
            if (_nextDependencyIndex < _currentDependencies.Length) {
                if (_newDependencies != null)
                    throw new Exception("_newDependencies unexpectedly initialized when _nextDependencyIndex is before the end");

                _newDependencies = new List<IReactiveData>();
                for (int i = 0; i < _nextDependencyIndex; ++i)
                    _newDependencies.Add(_currentDependencies[i]);
            }

            IReactiveData[] newDependenciesArray = _newDependencies.ToArray();

            var currDependenciesSet = new HashSet<IReactiveData>();
            foreach (IReactiveData currDependency in oldDependencies)
                currDependenciesSet.Add(currDependency);

            var newDependenciesSet = new HashSet<IReactiveData>();
            foreach (IReactiveData newDependency in newDependenciesArray)
                newDependenciesSet.Add(newDependency);

            // TODO: Catch exceptions here to ensure in consistent state
            foreach (IReactiveData removeDependency in currDependenciesSet.Except(newDependenciesSet))
                removeDependency.DataChanged -= dataChanged;

            foreach (IReactiveData addDependency in newDependenciesArray.Except(currDependenciesSet))
                addDependency.DataChanged += dataChanged;

            return newDependenciesArray;
        }
    }
}
