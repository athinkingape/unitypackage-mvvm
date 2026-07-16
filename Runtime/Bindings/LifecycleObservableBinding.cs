using System;

namespace MVVM.Bindings.Base
{
    public class LifecycleObservableBinding : ILifecycleBinding
    {
        private readonly Models.IObservable _observable;
        private readonly Action _onUpdate;

        public LifecycleObservableBinding(Models.IObservable observable, Action onUpdate)
        {
            _observable = observable;
            _onUpdate = onUpdate;
        }

        public void OnEnable()
        {
            _observable.Observe(_onUpdate);
        }

        public void OnDisable()
        {
            _observable.RemoveObservation(_onUpdate);
        }

        public void OnDestroy() => OnDisable();
    }

    public class LifecycleObservableBinding<T> : ILifecycleBinding
    {
        private readonly Models.IObservable<T> _observable;
        private readonly Action<T> _onUpdate;

        public LifecycleObservableBinding(Models.IObservable<T> observable, Action<T> onUpdate)
        {
            _observable = observable;
            _onUpdate = onUpdate;
        }

        public void OnEnable()
        {
            _observable.Observe(_onUpdate);
        }

        public void OnDisable()
        {
            _observable.RemoveObservation(_onUpdate);
        }

        public void OnDestroy() => OnDisable();
    }

    public class KeyObservableBinding<TKey, TValue> : ILifecycleBinding
    {
        private readonly Models.IKeyObservable<TKey, TValue> _observable;
        private readonly TKey _key;
        private readonly Action<TKey, TValue> _onUpdate;

        public KeyObservableBinding(Models.IKeyObservable<TKey, TValue> observable, TKey key, Action<TKey, TValue> onUpdate)
        {
            _observable = observable;
            _key = key;
            _onUpdate = onUpdate;
        }

        public void OnEnable()
        {
            _observable.Observe(_key, _onUpdate);
        }

        public void OnDisable()
        {
            _observable.RemoveObservation(_key, _onUpdate);
        }

        public void OnDestroy() => OnDisable();
    }
}
