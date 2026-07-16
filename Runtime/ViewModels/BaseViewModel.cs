using System;
using MVVM.Bindings.Base;
using MVVM.Models;
using UnityEngine;

namespace MVVM.ViewModels
{
    public abstract class BaseViewModel<T> : ObservableModel<T> where T : class
    {
        private bool _isEnabled;

        protected virtual void OnEnableImplementation() { }
        protected virtual void OnDisableImplementation() { }

        private protected override void OnBindingRegistered(ILifecycleBinding binding, Action reapply)
        {
            if (_isEnabled)
            {
                binding.OnEnable();
            }
            reapply?.Invoke();
        }

        internal void OnEnable()
        {
            if (_isEnabled || IsDestroyed)
            {
                return;
            }
            _isEnabled = true;

            for (int i = 0; i < _registeredBindings.Count; i++)
            {
                if (IsDestroyed || !_isEnabled)
                {
                    return;
                }

                _registeredBindings[i].binding.OnEnable();

                if (_registeredBindings[i].registrationFrame != Time.frameCount)
                {
                    _registeredBindings[i].reapply?.Invoke();
                }
            }

            OnEnableImplementation();
        }

        internal void OnDisable()
        {
            if (!_isEnabled)
            {
                return;
            }
            _isEnabled = false;

            for (int i = _registeredBindings.Count - 1; i >= 0; i--)
            {
                _registeredBindings[i].binding.OnDisable();
            }

            OnDisableImplementation();
        }

        private protected override void OnDestroyedInternal()
        {
            _isEnabled = false;
        }
    }
}
