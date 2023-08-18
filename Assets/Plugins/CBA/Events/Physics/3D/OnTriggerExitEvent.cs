using System;
using UnityEngine;

namespace CBA.Events.Physics._3D
{
    public class OnTriggerExitEvent : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LayerMask _layerMask;

        public event Action<Collider> onExit;

        private void OnTriggerExit(Collider other)
        {
            if (Extensions.LayerMask.ContainsLayer(_layerMask, other.gameObject.layer))
            {
                onExit?.Invoke(other);
            }
        }
    }
}
