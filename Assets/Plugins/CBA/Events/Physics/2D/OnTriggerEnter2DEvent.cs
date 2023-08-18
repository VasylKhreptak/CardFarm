using System;
using UnityEngine;

namespace CBA.Events.Physics._2D
{
    public class OnTriggerEnter2DEvent : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LayerMask _layerMask;

        public event Action<Collider2D> onEnter;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (Extensions.LayerMask.ContainsLayer(_layerMask, other.gameObject.layer))
            {
                onEnter?.Invoke(other);
            }
        }
    }
}
