namespace ReactiveData
{
    /// <summary>
    /// IObjectChanged allows its implementor to send a notification when it's changed in any way.
    /// IObjectChanged is similar to INotifyPropertyChanged except that it doesn't indicate which particular
    /// property changed. For ReactiveData either INotifyObjectChanged or INotifyPropertyChanged can be implemented
    /// on an object to indicate changes. INotifyPropertyChanged is generally used when needed for compatibility
    /// with other code that requires it, though ReactiveData doesn't pay attention to the particular property that
    /// changed, treating all changes the same. When compatibility isn't a concern then implementing
    /// INotifyObjectChanged is a bit simpler and more efficient, since no property is passed along.
    /// </summary>
    public interface INotifyObjectChanged
    {
        event ObjectChangedEventHandler ObjectChanged;
    }

    public delegate void ObjectChangedEventHandler();
}