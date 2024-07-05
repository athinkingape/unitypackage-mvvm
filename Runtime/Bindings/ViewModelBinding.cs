using MVVM.Models;
using MVVM.ViewModels;
using MVVM.Views;

namespace MVVM.Bindings.Base
{
    public class ViewModelBinding<TView, TViewModel> : BaseValueBinding<TViewModel>
        where TViewModel : BaseViewModel
        where TView : BaseView<TViewModel>
    {
        private readonly TView _view;
        
        public ViewModelBinding(IObservableValue<TViewModel> observableValue, TView view) : base(observableValue)
        {
            _view = view;
        }

        public ViewModelBinding(TView view, TViewModel value) : base(value)
        {
            _view = view;
        }

        protected override void OnUpdate(TViewModel value)
        {
            if (_view.ViewModel != value)
            {
                _view.Setup(value);
            }
        }
    }
}