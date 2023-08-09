using UnityEngine;

namespace Zenject.Installers.ProjectContext.ScriptableObjects.Core
{
    public class ScriptableObjectInstaller<T> : MonoInstaller where T : ScriptableObject
    {
        [Header("Preferences")]
        [SerializeField] private T _scriptableObject;

        public override void InstallBindings()
        {
            Container.BindInstance(_scriptableObject).AsSingle();
        }
    }
}
