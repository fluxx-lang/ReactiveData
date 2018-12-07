using System.Collections.Generic;

namespace ReactiveData
{
    public class Transaction
    {
        private readonly HashSet<DataChangedEventHandler> _toNotify = new HashSet<DataChangedEventHandler>();

        public void AddToNotify(DataChangedEventHandler dataChanged)
        {
            _toNotify.Add(dataChanged);
        }

        public void Complete()
        {
            foreach (DataChangedEventHandler dataChanged in _toNotify)
                dataChanged.Invoke();
        }
    }
}
