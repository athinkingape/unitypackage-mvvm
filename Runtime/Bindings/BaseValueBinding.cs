using MVVM.Models;

namespace MVVM.Bindings.Base
{
    public abstract class BaseValueBinding<T> : ILifecycleBinding
    {
        private readonly IObservableValue<T> _observableValue;
        private readonly T _value;

        public BaseValueBinding(IObservableValue<T> observableValue)
        {
            _observableValue = observableValue;
            _observableValue.Observe(OnUpdate);
        }

        public BaseValueBinding(T value)
        {
            _value = value;
        }

        public void OnEnable()
        {
            _observableValue?.Observe(OnUpdate);
            OnUpdate(_observableValue == null ? _value : _observableValue.Value);
        }

        public void OnDisable()
        {
            _observableValue?.RemoveObservation(OnUpdate);
        }

        public void OnDestroy()
        {
            _observableValue?.RemoveObservation(OnUpdate);
        }

        protected abstract void OnUpdate(T value);
    }
}