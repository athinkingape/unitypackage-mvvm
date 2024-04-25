using System;
using System.Collections.Generic;
using UnityEngine;

namespace MVVM.Models
{
    public abstract class BaseObservable<T> : IObservable<T>
    {
        private HashSet<Action<T>> _callbacks = new();
        private HashSet<Action<T>> _callbacksToRemove = new();

        private bool _isRunning;

        public void Observe(Action<T> OnUpdate)
        {
            if (_callbacks.Contains(OnUpdate))
            {
                return;
            }

            _callbacks.Add(OnUpdate);
        }

        public void RemoveObservation(Action<T> OnUpdate)
        {
            if (_isRunning)
            {
                _callbacksToRemove.Add(OnUpdate);
                return;
            }
            
            if (!_callbacks.Contains(OnUpdate))
            {
                return;
            }

            _callbacks.Remove(OnUpdate);
        }

        protected void NotifyObservers(T instance)
        {
            _isRunning = true;
            
            foreach (var callback in _callbacks)
            {
                callback.Invoke(instance);
            }

            _isRunning = false;

            foreach (var action in _callbacksToRemove)
            {
                RemoveObservation(action);
            }
            
            _callbacksToRemove.Clear();
        }
    }
}