using System;
using UnityEngine;

namespace CBA.Events.Physics._2D
{
    public class OnTriggerExit2DEvent : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LayerMask _layerMask;

        public event Action<Collider2D> onExit;

        private void OnTriggerExit2D(Collider2D other)
        {
            if (Extensions.LayerMask.ContainsLayer(_layerMask, other.gameObject.layer))
            {
                onExit?.Invoke(other);
            }
        }
    }
}
