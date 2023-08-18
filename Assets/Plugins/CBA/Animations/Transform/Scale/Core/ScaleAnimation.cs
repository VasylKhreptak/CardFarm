using CBA.Animations.Transform.Core;
using UnityEngine;

namespace CBA.Animations.Transform.Scale.Core
{
    public abstract class ScaleAnimation : TransformAnimation
    {
        [Header("Scale Preferences")]
        [SerializeField] protected Vector3 _startScale;
        [SerializeField] protected Vector3 _targetScale;
    }
}
