using System;
using UnityEngine;

namespace CBA.Events.Physics._3D
{
    public class OnTriggerEnterEvent : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LayerMask _layerMask;

        public event Action<Collider> onEnter;

        private void OnTriggerEnter(Collider other)
        {
            if (Extensions.LayerMask.ContainsLayer(_layerMask, other.gameObject.layer))
            {
                onEnter?.Invoke(other);
            }
        }
    }
}
