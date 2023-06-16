using UnityEngine;

namespace Graphics.Gizmos
{
    public class GizmosSphereDrawer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _transform;

        [Header("Preferences")]
        [SerializeField] private float _radius = 1;
        [SerializeField] private Color _color = Color.red;

        #region MonoBehaviour

        private void OnValidate()
        {
            _transform ??= GetComponent<Transform>();
        }

        #endregion

        private void OnDrawGizmos()
        {
            if (_transform == null) return;

            UnityEngine.Gizmos.color = _color;
            UnityEngine.Gizmos.DrawSphere(_transform.position, _radius);
        }
    }
}
