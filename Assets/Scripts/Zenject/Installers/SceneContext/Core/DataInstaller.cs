namespace Zenject.Installers.SceneContext.Core
{
    public class DataInstaller<T> : MonoInstaller where T : new()
    {
        public override void InstallBindings()
        {
            T data = new T();

            Container.BindInstance(data).AsSingle();
        }
    }
}
