using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ReactiveData {
    public abstract class ReactiveInpcObject : ReactiveBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected T Get<T>(T storage)
        {
            RunningDerivationsStack.Top?.AddDependency(this);
            return storage;
        }

        protected void Set<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
        {
            if (!Equals(storage, value)) {
                storage = value;

                // Notify via both the ReactiveData and INotifyPropertyChanged mechanisms
                NotifyChanged();  
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
