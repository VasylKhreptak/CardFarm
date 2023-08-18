using CBA.Actions.Core;
using NaughtyAttributes;
using UnityEngine;

namespace CBA.Animations.Actions.Global.Core
{
    public abstract class TargetOrientedAnimationAction : Action
    {
        [Header("Preferences")]
        [SerializeField] protected bool _useTarget;
        [HideIf(nameof(_useTarget)), SerializeField] protected int _id;
        [ShowIf(nameof(_useTarget)), SerializeField] protected Object _target;
    }
}
