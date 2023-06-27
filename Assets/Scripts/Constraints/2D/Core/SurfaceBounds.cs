using UnityEngine;

namespace Constraints._2D.Core
{
    public class SurfaceBounds : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _transform;

        [Header("Preferences")]
        [SerializeField] private Vector2 _min;
        [SerializeField] private Vector2 _max;

        #region MonoBehaviour

        private void OnValidate()
        {
            _transform ??= GetComponent<Transform>();
        }

        #endregion

        public bool IsInside(Vector3 point)
        {
            Vector2 minWorld = _transform.TransformPoint(_min);
            Vector2 maxWorld = _transform.TransformPoint(_max);

            return point.x >= minWorld.x && point.x <= maxWorld.x &&
                point.z >= minWorld.y && point.z <= maxWorld.y;
        }

        public bool IsInside(Bounds bounds)
        {
            Vector2 minWorld = _transform.TransformPoint(_min);
            Vector2 maxWorld = _transform.TransformPoint(_max);

            Vector3 min = bounds.min;
            Vector3 max = bounds.max;

            return min.x >= minWorld.x && max.x <= maxWorld.x &&
                min.z >= minWorld.y && max.z <= maxWorld.y;
        }

        public Vector3 Clamp(Vector3 point)
        {
            Vector2 minWorld = _transform.TransformPoint(_min);
            Vector2 maxWorld = _transform.TransformPoint(_max);

            float x = Mathf.Clamp(point.x, minWorld.x, maxWorld.x);
            float z = Mathf.Clamp(point.z, minWorld.y, maxWorld.y);

            return new Vector3(x, point.y, z);
        }

        public Vector3 Clamp(Bounds bounds)
        {
            Vector2 minWorld = _transform.TransformPoint(_min);
            Vector2 maxWorld = _transform.TransformPoint(_max);

            float x = Mathf.Clamp(bounds.center.x, minWorld.x + bounds.extents.x, maxWorld.x - bounds.extents.x);
            float z = Mathf.Clamp(bounds.center.z, minWorld.y + bounds.extents.z, maxWorld.y - bounds.extents.z);

            return new Vector3(x, bounds.center.y, z);
        }

        public Vector3 GetRandomPoint()
        {
            Vector2 minWorld = _transform.TransformPoint(_min);
            Vector2 maxWorld = _transform.TransformPoint(_max);

            float x = Random.Range(minWorld.x, maxWorld.x);
            float z = Random.Range(minWorld.y, maxWorld.y);

            return new Vector3(x, _transform.position.y, z);
        }

        public Vector3 GetRandomPoint(Bounds bounds)
        {
            Vector2 minWorld = _transform.TransformPoint(_min);
            Vector2 maxWorld = _transform.TransformPoint(_max);

            float x = Random.Range(minWorld.x + bounds.extents.x, maxWorld.x - bounds.extents.x);
            float z = Random.Range(minWorld.y + bounds.extents.z, maxWorld.y - bounds.extents.z);

            return new Vector3(x, _transform.position.y, z);
        }

        public Vector3 GetRandomPositionInRange(Bounds bounds, float range)
        {
            Vector3 position = Vector3.zero;

            Vector2 insideUnitCircle = Random.insideUnitCircle.normalized * range;

            Vector3 randomSphere = new Vector3(insideUnitCircle.x, 0f, insideUnitCircle.y);

            position = bounds.center + randomSphere;
            bounds.center = position;
            position = Clamp(bounds);
            return position;
        }

        public Vector3 GetRandomPositionInRange(Bounds bounds, float minRange, float maxRange)
        {
            float range = Random.Range(minRange, maxRange);
            return GetRandomPositionInRange(bounds, range);
        }

        private void OnDrawGizmos()
        {
            if (_transform == null) return;


            Vector2 minWorld = _transform.TransformPoint(_min);
            Vector2 maxWorld = _transform.TransformPoint(_max);

            float y = _transform.position.y;

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(new Vector3(minWorld.x, y, minWorld.y), new Vector3(minWorld.x, y, maxWorld.y));
            Gizmos.DrawLine(new Vector3(minWorld.x, y, maxWorld.y), new Vector3(maxWorld.x, y, maxWorld.y));
            Gizmos.DrawLine(new Vector3(maxWorld.x, y, maxWorld.y), new Vector3(maxWorld.x, y, minWorld.y));
            Gizmos.DrawLine(new Vector3(maxWorld.x, y, minWorld.y), new Vector3(minWorld.x, y, minWorld.y));
        }
    }
}
