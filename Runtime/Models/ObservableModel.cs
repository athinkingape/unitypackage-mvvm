using System.Collections.Generic;
using MVVM.Bindings.Base;

namespace MVVM.Models
{
    public class ObservableModel<T> : BaseObservable<T> 
        where T : class
    {
        private readonly List<IDestroyableBinding> _bindings = new();
        
        protected void Register<V>(IObservable<V> observable)
        {
            _bindings.Add(new ObservableBinding<V>(observable, v => NotifyObservers(this as T)));
        }

        public void Destroy()
        {
            foreach (var binding in _bindings)
            {
                binding.OnDestroy();
            }
            
            _bindings.Clear();
        }
    }
}