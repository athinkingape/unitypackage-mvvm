using System;

namespace MVVM.ViewModels
{
    public abstract class BaseViewModel
    {
        public void Observe<T>(Models.IObservable<T> model, Action<T> OnUpdate)
        {
            model.Observe(OnUpdate);
        }

         public void RemoveObservation<T>(Models.IObservable<T> model, Action<T> OnUpdate)
        {
            model.RemoveObservation(OnUpdate);
        }
        
        internal void OnEnable() { }
        internal void OnDisable() { }
        internal void OnDestroy() { }
    }
}