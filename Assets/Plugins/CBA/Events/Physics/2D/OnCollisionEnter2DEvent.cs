using System;
using UnityEngine;

namespace CBA.Events.Physics._2D
{
    public class OnCollisionEnter2DEvent : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LayerMask _layerMask;

        public event Action<Collision2D> onEnter;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (Extensions.LayerMask.ContainsLayer(_layerMask, collision.gameObject.layer))
            {
                onEnter?.Invoke(collision);
            }
        }
    }
}
