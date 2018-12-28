using System;
using System.Collections.Generic;

namespace ReactiveData
{
    public static class Transaction
    {
        [ThreadStatic] private static HashSet<DataChangedEventHandler> _toNotify;

        public static void Start()
        {
            _toNotify = new HashSet<DataChangedEventHandler>();
        }

        public static void AddToNotify(DataChangedEventHandler dataChanged)
        {
            _toNotify.Add(dataChanged);
        }

        public static void Complete()
        {
            foreach (DataChangedEventHandler dataChanged in _toNotify)
                dataChanged.Invoke();
            _toNotify = null;
        }
    }
}
