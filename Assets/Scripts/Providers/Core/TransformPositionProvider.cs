using UnityEngine;

namespace Providers.Core
{
    public class TransformPositionProvider : Provider<Vector3>
    {
        [Header("References")]
        [SerializeField] private Transform _transform;

        #region MonoBehaviour

        private void OnValidate()
        {
            _transform ??= GetComponent<Transform>();
        }

        #endregion

        public override Vector3 Value => _transform.position;
    }
}
