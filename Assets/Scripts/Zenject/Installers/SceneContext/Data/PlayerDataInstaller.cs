using Data.Player.Core;

namespace Zenject.Installers.SceneContext.Data
{
    public class PlayerDataInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            PlayerData playerData = new PlayerData();

            Container.BindInstance(playerData).AsSingle();
        }
    }
}
