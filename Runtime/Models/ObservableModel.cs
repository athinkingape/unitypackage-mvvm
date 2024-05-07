namespace MVVM.Models
{
    public class ObservableModel<T> : BaseObservable<T>, IObservableValue<T>
        where T : IObservable<T>
    {
        public T Value { get; private set; }

        public ObservableModel(T value)
        {
            Setup(value);
        }

        public void Setup(T value)
        {
            if (Value != null && Value.Equals(value))
            {
                return;
            }

            if (Value != null)
            {
                Value.RemoveObservation(OnValueUpdated);
            }
            
            Value = value;
            Value.Observe(OnValueUpdated);
            
            NotifyObservers(value);
        }

        private void OnValueUpdated(T obj)
        {
            NotifyObservers(obj);
        }
    }
}