using CBA.Animations.Transform.Core;
using UnityEngine;

namespace CBA.Animations.Transform.Move.Jump.Core
{
    public abstract class JumpAnimation : TransformAnimation
    {
        [Header("Jump Preferences")]
        [SerializeField] protected Vector3 _startPosition;
        [SerializeField] protected Vector3 _targetPosition;
        [SerializeField] protected float _power = 2f;
        [SerializeField] protected int _jumpsNumber = 1;

        [Header("Snapping")]
        [SerializeField] protected bool _snapping;
    }
}
