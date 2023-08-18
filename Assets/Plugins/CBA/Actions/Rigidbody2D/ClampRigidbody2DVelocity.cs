using CBA.Actions.Rigidbody2D.Core;
using UnityEngine;

namespace CBA.Actions.Rigidbody2D
{
    public class ClampRigidbody2DVelocity : Rigidbody2DAction
    {
        [Header("Preferences")]
        [SerializeField] private float _maxVelocity;

        public override void Do()
        {
            _rigidbody2D.velocity = Vector2.ClampMagnitude(_rigidbody2D.velocity, _maxVelocity);
        }
    }
}
