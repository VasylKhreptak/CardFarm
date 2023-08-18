using CBA.Animations.Transform.Core;
using CBA.Animations.Transform.Move.Core;
using UnityEngine;

namespace CBA.Animations.Transform.Rotate.Core
{
    public abstract class RotateAnimation : TransformAnimation
    {
        [Header("Rotate Preferences")]
        [SerializeField] protected Vector3 _startAngle;
        [SerializeField] protected Vector3 _targetAngle;
    }
}
