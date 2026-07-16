using System;

namespace MVVM.Models
{
    public interface IKeyObservable<TKey, TValue>
    {
        TValue Get(TKey key);
        void Observe(TKey key, Action<TKey, TValue> onValueChanged);
        void RemoveObservation(TKey key, Action<TKey, TValue> onValueChanged);
    }
}
