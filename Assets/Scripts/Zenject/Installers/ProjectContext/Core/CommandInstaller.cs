using Runtime.Commands.Core;

namespace Zenject.Installers.ProjectContext.Core
{
    public class CommandInstaller<T> : MonoInstaller where T : Command, new()
    {
        public override void InstallBindings()
        {
            T instance = new T();
            Container.BindInstance(instance).AsSingle();
        }
    }
}
