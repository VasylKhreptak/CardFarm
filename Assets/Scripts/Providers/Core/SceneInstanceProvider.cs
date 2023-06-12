using UnityEngine;

namespace Providers.Core
{
    public class SceneInstanceProvider<T> : Provider<T> where T : Object
    {
        [Header("References")]
        [SerializeField] private T _target;

        #region MonoBehaviour

        private void OnValidate()
        {
            _target ??= FindObjectOfType<T>();
        }

        #endregion

        public override T Value => _target;
    }
}
