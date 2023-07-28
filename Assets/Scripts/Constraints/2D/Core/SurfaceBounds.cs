using Tools.Bounds;
using UnityEngine;

namespace Constraints._2D.Core
{
    public class SurfaceBounds : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform _bounds;

        [Header("Preferences")]
        [SerializeField] private Color _gizmosColor = Color.blue;

        public RectTransform Bounds => _bounds;
        
        #region MonoBehaviour

        private void OnValidate()
        {
            _bounds ??= GetComponent<RectTransform>();
        }

        #endregion

        public bool IsInside(RectTransform innerRect)
        {
            return _bounds.IsInside(innerRect);
        }

        public Vector3 Clamp(RectTransform innerRect)
        {
            return _bounds.Clamp(innerRect);
        }

        public Vector3 GetRandomPoint(RectTransform innerRect)
        {
            return _bounds.GetRandomPoint(innerRect);
        }

        public Vector3 Clamp(RectTransform innerRect, Vector3 targetRectPosition)
        {
            return _bounds.Clamp(innerRect, targetRectPosition);
        }
        
        public Vector3 GetRandomPositionInRange(RectTransform innerRect, float range)
        {
            Vector3 position = Vector3.zero;

            Vector2 insideUnitCircle = Random.insideUnitCircle.normalized * range;

            Vector3 randomSphere = new Vector3(insideUnitCircle.x, 0f, insideUnitCircle.y);

            position = innerRect.position + randomSphere;

            return _bounds.Clamp(innerRect, position);
        }

        public Vector3 GetRandomPositionInRange(RectTransform innerRect, float minRange, float maxRange)
        {
            float range = Random.Range(minRange, maxRange);
            return GetRandomPositionInRange(innerRect, range);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = _gizmosColor;
            Gizmos.DrawWireCube(_bounds.position, _bounds.rect.size);
        }
    }
}
