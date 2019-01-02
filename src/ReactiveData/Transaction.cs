using System;
using System.Collections.Generic;

namespace ReactiveData
{
    public static class Transaction
    {
        [ThreadStatic] private static HashSet<ChangedEventHandler> _reactionsToNotify;

        public static void Start()
        {
            _reactionsToNotify = new HashSet<ChangedEventHandler>();
        }

        public static void AddToNotify(ChangedEventHandler changedEventHandler)
        {
            _reactionsToNotify.Add(changedEventHandler);
        }

        public static void Complete()
        {
            foreach (ChangedEventHandler changedEventHandler in _reactionsToNotify)
                changedEventHandler.Invoke();
            _reactionsToNotify = null;
        }

        public static void Run(Action action)
        {
            Start();
            action.Invoke();
            Complete();
        }

        public static void EnsureInTransaction()
        {
            if (_reactionsToNotify == null)
                throw new InvalidOperationException("Reactive data can only be modified inside a transaction. Use Transaction.Start / Transaction.Complete around updates.");
        }
    }
}
