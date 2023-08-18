using System;
using UnityEngine;

namespace CBA.Events.Physics._2D
{
    public class OnCollisionExit2DEvent : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LayerMask _layerMask;

        public event Action<Collision2D> onExit;

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (Extensions.LayerMask.ContainsLayer(_layerMask, collision.gameObject.layer))
            {
                onExit?.Invoke(collision);
            }
        }
    }
}
