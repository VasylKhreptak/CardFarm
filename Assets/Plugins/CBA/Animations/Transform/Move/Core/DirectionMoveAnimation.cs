using CBA.Animations.Transform.Core;
using UnityEngine;

namespace CBA.Animations.Transform.Move.Core
{
    public abstract class DirectionMoveAnimation : TransformAnimation
    {
        [Header("Move Preferences")]
        [SerializeField] protected float _from;
        [SerializeField] protected float _to;

        [Header("Snapping")]
        [SerializeField] protected bool _snapping;

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
