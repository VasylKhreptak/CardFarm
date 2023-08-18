using System;
using UnityEngine;

namespace CBA.Events.Physics._3D
{
    public class OnCollisionStayEvent : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LayerMask _layerMask;

        public event Action<Collision> onStay;

        private void OnCollisionStay(Collision collision)
        {
            if (Extensions.LayerMask.ContainsLayer(_layerMask, collision.gameObject.layer))
            {
                onStay?.Invoke(collision);
            }
        }
    }
}
