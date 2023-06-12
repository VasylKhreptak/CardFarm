using UnityEngine;

namespace Zenject.Installers.ProjectContext.Core
{
    public class ProjectInstanceCreatorInstaller<T> : MonoInstaller
    {
        [Header("References")]
        [SerializeField] private GameObject _prefab;

        #region MonoBehaviour

        private void OnValidate()
        {
            if (_prefab != null && _prefab.TryGetComponent(out T _) == false)
            {
                Debug.LogError($"Prefab {_prefab.name} does not contain component of type {typeof(T)}");
                
                _prefab = null;
            }
        }

        #endregion

        public override void InstallBindings()
        {
            GameObject instance = Instantiate(_prefab, transform);
            Container.Bind<T>().FromComponentOn(instance).AsSingle();
        }
    }
}
