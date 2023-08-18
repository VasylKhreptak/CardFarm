using UnityEngine;

namespace CBA.Animations.Physics.Rigidbody.Move.Core
{
    public abstract class RigidbodyDirectionMoveAnimation : RigidbodyMoveAnimation
    {
        [Header("Move Preferences")]
        [SerializeField] protected float _from;
        [SerializeField] protected float _to;

        public override void MoveToStartState()
        {
            MoveTo(_from);
        }

        public override void MoveToEndState()
        {
            MoveTo(_to);
        }

        protected abstract void MoveTo(float axisPosition);
    }
}
