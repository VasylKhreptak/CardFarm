using System;
using UnityEngine;

namespace CBA.Events.Physics._2D
{
    public class OnTriggerStay2DEvent : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LayerMask _layerMask;

        public event Action<Collider2D> onStay;

        private void OnTriggerStay2D(Collider2D other)
        {
            if (Extensions.LayerMask.ContainsLayer(_layerMask, other.gameObject.layer))
            {
                onStay?.Invoke(other);
            }
        }
    }
}
