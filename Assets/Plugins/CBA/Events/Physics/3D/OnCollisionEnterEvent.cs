using System;
using UnityEngine;

namespace CBA.Events.Physics._3D
{
    public class OnCollisionEnterEvent : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LayerMask _layerMask;

        public event Action<Collision> onEnter;

        private void OnCollisionEnter(Collision collision)
        {
            if (Extensions.LayerMask.ContainsLayer(_layerMask, collision.gameObject.layer))
            {
                onEnter?.Invoke(collision);
            }
        }
    }
}
