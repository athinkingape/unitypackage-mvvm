using System;

namespace MVVM.Bindings.Base
{
    public class ObservableBinding : LifecycleObservableBinding
    {
        public ObservableBinding(Models.IObservable observable, Action onUpdate) : base(observable, onUpdate)
        {
            OnEnable();
        }
    }

    public class ObservableBinding<T> : LifecycleObservableBinding<T>
    {
        public ObservableBinding(Models.IObservable<T> observable, Action<T> onUpdate) : base(observable, onUpdate)
        {
            OnEnable();
        }
    }
}
