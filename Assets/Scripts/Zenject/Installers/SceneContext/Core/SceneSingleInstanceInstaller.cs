using UnityEngine;

namespace Zenject.Installers.SceneContext.Core
{
    public class SceneSingleInstanceInstaller<T> : MonoInstaller where T : Object
    {
        [Header("References")]
        [SerializeField] private T _objectToBind;

        #region MonoBehaviour

        private void OnValidate()
        {
            _objectToBind ??= FindObjectOfType<T>();
        }

        #endregion

        public override void InstallBindings()
        {
            Container.BindInstance(_objectToBind).AsSingle();
        }
    }
}
