using UniRx;
using UnityEngine;

namespace Graphics.Lighting
{
    public class LightData : MonoBehaviour
    {
        public Vector3ReactiveProperty Direction = new Vector3ReactiveProperty();

        private void OnDrawGizmosSelected()
        {
            UnityEngine.Gizmos.color = Color.yellow;
            UnityEngine.Gizmos.DrawRay(transform.position, Direction.Value.normalized);
        }
    }
}
