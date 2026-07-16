using System;
using System.Collections.Generic;
using MVVM.Bindings.Base;
using UnityEngine;

namespace MVVM.Models
{
    public interface IDestructibleModel {
        public void OnDestroy();
    }

    public class ObservableModel<T> : BaseObservable<T>, IDestructibleModel
        where T : class
    {
        private protected readonly List<(ILifecycleBinding binding, Action reapply, int registrationFrame)> _registeredBindings = new();
        private bool _isDestroyed;

        protected bool IsDestroyed => _isDestroyed;

        private protected virtual void OnBindingRegistered(ILifecycleBinding binding, Action reapply)
        {
            binding.OnEnable();
            reapply?.Invoke();
        }

        private void Register(ILifecycleBinding binding, Action reapply)
        {
            _registeredBindings.Add((binding, reapply, Time.frameCount));
            OnBindingRegistered(binding, reapply);
        }

        protected void RegisterChildObservable<V>(IObservable<V> observable)
        {
            Register(new LifecycleObservableBinding<V>(observable, v => NotifyObservers(this as T)), null);
        }

        protected void Observe(IObservable observable, Action onNotify, bool updateImmediately = false) {
            Register(new LifecycleObservableBinding(observable, onNotify),
                updateImmediately ? onNotify : null);
        }

        protected void Observe<A>(Models.IObservable<A> observable, Action<A> onUpdate, bool updateImmediately = false)
        {
            Register(new LifecycleObservableBinding<A>(observable, onUpdate),
                updateImmediately ? () => onUpdate((A)observable) : null);
        }

        protected void Observe<A>(IObservableValue<A> observable, Action<A> onUpdate, bool updateImmediately = false)
        {
            Register(new LifecycleObservableBinding<A>(observable, onUpdate),
                updateImmediately ? () => onUpdate(observable.Value) : null);
        }

        protected void Observe<TKey, TValue>(IKeyObservable<TKey, TValue> observable, TKey key, Action<TKey, TValue> onUpdate, bool updateImmediately = false)
        {
            Register(new KeyObservableBinding<TKey, TValue>(observable, key, onUpdate),
                updateImmediately ? () => onUpdate(key, observable.Get(key)) : null);
        }

        protected void ObserveAny<A, B>(IObservableValue<A> observableA, IObservableValue<B> observableB, Action<A, B> onUpdate, bool updateImmediately = false)
        {
            void Notify()
            {
                onUpdate(observableA.Value, observableB.Value);
            }

            Register(new LifecycleObservableBinding<A>(observableA, a => Notify()), null);
            Register(new LifecycleObservableBinding<B>(observableB, b => Notify()),
                updateImmediately ? Notify : null);
        }

        protected void ObserveAny<A, B, C>(IObservableValue<A> observableA, IObservableValue<B> observableB, IObservableValue<C> observableC, Action<A, B, C> onUpdate, bool updateImmediately = false)
        {
            void Notify()
            {
                onUpdate(observableA.Value, observableB.Value, observableC.Value);
            }

            Register(new LifecycleObservableBinding<A>(observableA, a => Notify()), null);
            Register(new LifecycleObservableBinding<B>(observableB, b => Notify()), null);
            Register(new LifecycleObservableBinding<C>(observableC, c => Notify()),
                updateImmediately ? Notify : null);
        }

        public void OnDestroy()
        {
            if (_isDestroyed)
            {
                return;
            }
            _isDestroyed = true;

            foreach (var entry in _registeredBindings)
            {
                entry.binding.OnDestroy();
            }
            _registeredBindings.Clear();

            ClearObservers();
            OnDestroyedInternal();
            OnDestroyImplementation();
        }

        private protected virtual void OnDestroyedInternal() { }

        protected virtual void OnDestroyImplementation() { }
    }
}
