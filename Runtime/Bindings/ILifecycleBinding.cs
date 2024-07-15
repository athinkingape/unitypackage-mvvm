namespace MVVM.Bindings.Base
{
    public interface ILifecycleBinding : IDestroyableBinding
    {
        public void OnEnable();
        public void OnDisable();
    }
}