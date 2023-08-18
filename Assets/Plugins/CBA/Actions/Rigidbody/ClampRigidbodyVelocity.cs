using CBA.Actions.Rigidbody.Core;
using UnityEngine;

namespace CBA.Actions.Rigidbody
{
    public class ClampRigidbodyVelocity : RigidbodyAction
    {
        [Header("Preferences")]
        [SerializeField] private float _maxVelocity;

        public override void Do()
        {
            _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, _maxVelocity);
        }
    }
}
