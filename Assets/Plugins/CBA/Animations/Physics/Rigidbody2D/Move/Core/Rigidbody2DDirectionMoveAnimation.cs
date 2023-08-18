using CBA.Animations.Physics.Rigidbody2D.Core;
using UnityEngine;

namespace CBA.Animations.Physics.Rigidbody2D.Move.Core
{
    public abstract class Rigidbody2DDirectionMoveAnimation : Rigidbody2DMoveAnimation
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
