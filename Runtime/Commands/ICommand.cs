namespace MVVM.Commands
{
    public interface ICommand
    {
        void Execute();
    }
    
    public interface ICommand<T>
    {
        void Execute(T parameter);
    }
}