namespace MVVM.Bindings.Base
{
    public interface ILifecycleBinding
    {
        public void OnEnable();
        public void OnDisable();
        public void OnDestroy();
    }
}