using CBA.Animations.Transform.Core;
using UnityEngine;

namespace CBA.Animations.Transform.Move.Core
{
    public abstract class MoveAnimation : TransformAnimation
    {
        [Header("Move Preferences")]
        [SerializeField] protected Vector3 _startPosition;
        [SerializeField] protected Vector3 _targetPosition;

        [Header("Snapping")]
        [SerializeField] protected bool _snapping;
    }
}
