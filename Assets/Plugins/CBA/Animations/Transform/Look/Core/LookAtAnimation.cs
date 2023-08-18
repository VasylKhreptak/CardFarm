using CBA.Animations.Transform.Core;
using CBA.Animations.Transform.Move.PositionProvider.Core;
using NaughtyAttributes;
using UnityEngine;

namespace CBA.Animations.Transform.Look.Core
{
    public abstract class LookAtAnimation: TransformAnimation
    {
        [Header("Look At Preferences")]
        [Required, SerializeField] protected PositionProvider _starTarget;
        [Required, SerializeField] protected PositionProvider _endTarget;
        [SerializeField] protected DG.Tweening.AxisConstraint _axisConstraint = DG.Tweening.AxisConstraint.None;
        [SerializeField] protected Vector3 _up = Vector3.up;
    }
}
