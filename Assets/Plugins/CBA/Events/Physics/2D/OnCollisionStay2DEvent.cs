using System;
using UnityEngine;

namespace CBA.Events.Physics._2D
{
    public class OnCollisionStay2DEvent : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LayerMask _layerMask;

        public event Action<Collision2D> onStay;

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (Extensions.LayerMask.ContainsLayer(_layerMask, collision.gameObject.layer))
            {
                onStay?.Invoke(collision);
            }
        }
    }
}
