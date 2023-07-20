using UnityEngine;

namespace TransformManagement
{
    public class ScaleReseter : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _transform;

        [Header("Preferences")]
        [SerializeField] private Vector3 _targetScale = Vector3.one;

        #region MonoBehaviour

        private void OnValidate()
        {
            _transform ??= GetComponent<Transform>();
        }

        private void OnDisable()
        {
            ResetScale();
        }

        #endregion

        private void ResetScale()
        {
            _transform.localScale = _targetScale;
        }
    }
}
