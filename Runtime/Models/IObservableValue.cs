namespace MVVM.Models
{
    public interface IObservableValue<T> : IObservable<T>
    {
        T Value { get; }
    }
}