using UnityEngine;

namespace CBA.Animations.Core
{
    public abstract class Animation : AnimationCore
    {
        [Header("Duration")]
        [SerializeField] protected float _duration = 1f;
    }
}
