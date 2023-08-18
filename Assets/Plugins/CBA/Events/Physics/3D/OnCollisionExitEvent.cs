using System;
using UnityEngine;

namespace CBA.Events.Physics._3D
{
    public class OnCollisionExitEvent : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LayerMask _layerMask;

        public event Action<Collision> onExit;

        private void OnCollisionExit(Collision collision)
        {
            if (Extensions.LayerMask.ContainsLayer(_layerMask, collision.gameObject.layer))
            {
                onExit?.Invoke(collision);
            }
        }
    }
}
