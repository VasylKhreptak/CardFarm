using CBA.Animations.Transform.Core;
using UnityEngine;

namespace CBA.Animations.Transform.Scale.Core
{
    public abstract class DirectionScaleAnimation : TransformAnimation
    {
        [Header("Scale Preferences")]
        [SerializeField] protected float _from;
        [SerializeField] protected float _to;
    }
}
