using System;

namespace MVVM.Bindings.Base
{
    internal class ObservableBinding<T> : IDestroyableBinding
    {
        private readonly Models.IObservable<T> _observable;
        private readonly Action<T> _onUpdate;
        
        public ObservableBinding(Models.IObservable<T> observable, Action<T> onUpdate)
        {
            _observable = observable;
            _onUpdate = onUpdate;
            
            observable.Observe(onUpdate);
        }

        public void OnDestroy()
        {
            _observable.RemoveObservation(_onUpdate);
        }
    }
}