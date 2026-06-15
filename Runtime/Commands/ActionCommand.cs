using System;

namespace MVVM.Commands {
    public class ActionCommand : ICommand {
        private readonly Action _executeAction;

        public ActionCommand(Action executeAction) {
            _executeAction = executeAction;
        }
        
        public void Execute() {
            _executeAction?.Invoke();
        }
    }

    public class ActionCommand<T> : ICommand<T> {
        private readonly Action<T> _executeAction;

        public ActionCommand(Action<T> executeAction) {
            _executeAction = executeAction;
        }
        
        public void Execute(T parameter) {
            _executeAction?.Invoke(parameter);
        }
    }
}