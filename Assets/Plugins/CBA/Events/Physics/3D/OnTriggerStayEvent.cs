using System;
using UnityEngine;

namespace CBA.Events.Physics._3D
{
    public class OnTriggerStayEvent : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LayerMask _layerMask;

        public event Action<Collider> onStay;

        private void OnTriggerStay(Collider other)
        {
            if (Extensions.LayerMask.ContainsLayer(_layerMask, other.gameObject.layer))
            {
                onStay?.Invoke(other);
            }
        }
    }
}
