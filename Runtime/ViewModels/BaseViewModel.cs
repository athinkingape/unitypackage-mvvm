using System;
using System.Collections.Generic;
using MVVM.Bindings.Base;
using MVVM.Models;

namespace MVVM.ViewModels
{
    public abstract class BaseViewModel
    {
        private List<IDestroyableBinding> _bindingsToDestroy = new();
        
        protected void Observe<T>(Models.IObservable<T> observable, Action<T> onUpdate)
        {
            _bindingsToDestroy.Add(new ObservableBinding<T>(observable, onUpdate));
        }
        
        protected void Observe<T>(IObservableValue<T> observable, Action<T> onUpdate, bool updateImmediately = false)
        {
            _bindingsToDestroy.Add(new ObservableBinding<T>(observable, onUpdate));

            if (updateImmediately)
            {
                onUpdate?.Invoke(observable.Value);
            }
        }

        protected void ObserveAny<A, B>(IObservableValue<A> observableA, IObservableValue<B> observableB, Action<A, B> onUpdate, bool updateImmediately = false)
        {
            void Notify()
            {
                onUpdate(observableA.Value, observableB.Value);
            }
            
            _bindingsToDestroy.Add(new ObservableBinding<A>(observableA, a => Notify()));
            _bindingsToDestroy.Add(new ObservableBinding<B>(observableB, b => Notify()));

            if (updateImmediately)
            {
                Notify();
            }
        }
        
        protected void ObserveAny<A, B, C>(IObservableValue<A> observableA, IObservableValue<B> observableB, IObservableValue<C> observableC, Action<A, B, C> onUpdate, bool updateImmediately = false)
        {
            void Notify()
            {
                onUpdate(observableA.Value, observableB.Value, observableC.Value);
            }
            
            _bindingsToDestroy.Add(new ObservableBinding<A>(observableA, a => Notify()));
            _bindingsToDestroy.Add(new ObservableBinding<B>(observableB, b => Notify()));
            _bindingsToDestroy.Add(new ObservableBinding<C>(observableC, c => Notify()));

            if (updateImmediately)
            {
                Notify();
            }
        }

        /*
         * DEPRECATED
         */
        protected void RemoveObservation<T>(Models.IObservable<T> model, Action<T> onUpdate)
        {
            model.RemoveObservation(onUpdate);
        }
        
        protected virtual void OnEnableImplementation() { }
        protected virtual void OnDisableImplementation() { }
        protected virtual void OnDestroyImplementation() { }

        internal void OnEnable()
        {
            OnEnableImplementation();
        }

        internal void OnDisable()
        {
            OnDisableImplementation();
        }

        internal void OnDestroy()
        {
            foreach (var destroyableBinding in _bindingsToDestroy)
            {
                destroyableBinding.OnDestroy();
            }
            
            _bindingsToDestroy.Clear();
            
            OnDestroyImplementation();
        }
    }
}