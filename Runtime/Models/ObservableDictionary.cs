using System;
using System.Collections.Generic;

namespace MVVM.Models
{
    public class ObservableDictionary<TKey, TValue> : BaseObservable<ObservableDictionary<TKey, TValue>>
    {
        private Dictionary<TKey, TValue> _dict = new();
        private Dictionary<TKey, HashSet<Action<TKey, TValue>>> _observers = new();

        public void Add(TKey key, TValue value)
        {
            var changed = !_dict.ContainsKey(key) || !_dict[key].Equals(value);
            if (!changed)
            {
                return;
            }

            _dict[key] = value;

            if (_observers.ContainsKey(key))
            {
                NotifyObservers(_observers[key], key, value);
            }
            
            NotifyObservers(this);
        }

        public void Remove(TKey key)
        {
            var changed = !_dict.ContainsKey(key);
            if (!changed)
            {
                return;
            }

            _dict.Remove(key);

            if (_observers.ContainsKey(key))
            {
                NotifyObservers(_observers[key], key, default);
            }
            
            NotifyObservers(this);
        }

        public TValue Get(TKey key) => _dict.GetValueOrDefault(key);
        
        public bool Contains(TKey key) => _dict.ContainsKey(key);

        public void Observe(TKey key, Action<TKey, TValue> onValueChanged)
        {
            if (_observers.ContainsKey(key) && _observers[key].Contains(onValueChanged))
            {
                return;
            }

            if (!_observers.TryGetValue(key, out HashSet<Action<TKey, TValue>> observers))
            {
                observers = new HashSet<Action<TKey, TValue>>();
            }

            observers.Add(onValueChanged);
        }

        public void RemoveObservation(TKey key, Action<TKey, TValue> onValueChanged)
        {
            if (!_observers.ContainsKey(key) || !_observers[key].Contains(onValueChanged))
            {
                return;
            }

            _observers[key].Remove(onValueChanged);
        }
        
        private void NotifyObservers(HashSet<Action<TKey, TValue>> observers, TKey key, TValue value)
        {
            foreach (var observer in observers)
            {
                observer.Invoke(key, value);
            }
        }
    }
}