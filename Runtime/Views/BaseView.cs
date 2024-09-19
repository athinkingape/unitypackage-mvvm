using System.Collections.Generic;
using MVVM.Bindings.Base;
using MVVM.ViewModels;
using UnityEngine;

namespace MVVM.Views
{
    public abstract class BaseView<TViewModel> : MonoBehaviour 
        where TViewModel : BaseViewModel
    {
        public TViewModel ViewModel { get; private set; }

        private List<ILifecycleBinding> _bindings = new();

        public void Setup(TViewModel viewModel)
        {
            if (ViewModel != null)
            {
                DestroyViewModel();
                ViewModel = null;
            } 
            
            ViewModel = viewModel;
            OnSetup(viewModel);
        }

        protected abstract void OnSetup(TViewModel viewModel);

        protected virtual void OnEnableImpl() { }

        protected virtual void OnDisableImpl() { }
        protected virtual void OnDestroyImpl() { }

        private void OnEnable()
        {
            _bindings.ForEach(b => b.OnEnable());
            ViewModel?.OnEnable();
            OnEnableImpl();
        }

        private void OnDisable()
        {
            OnDisableImpl();
            _bindings.ForEach(b => b.OnDisable());
            ViewModel?.OnDisable();
        }

        private void OnDestroy()
        {
            OnDestroyImpl();
            DestroyViewModel();
        }

        private void DestroyViewModel()
        {
            _bindings.ForEach(b => b.OnDestroy());
            _bindings.Clear();
            ViewModel?.OnDestroy();
        }

        protected void Bind(ILifecycleBinding valueBinding)
        {
            _bindings.Add(valueBinding);
            valueBinding.OnEnable();
        }
    }
}