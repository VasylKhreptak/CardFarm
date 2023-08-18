using CBA.Animations.Actions.Global.Core;
using DG.Tweening;
using UnityEngine;

namespace CBA.Animations.Actions.Global
{
    public class RewindAnimations : TargetOrientedAnimationAction
    {
        [Header("Rewind Preferences")]
        [SerializeField] private bool _includeDelay = true;
        
        public override void Do()
        {
            DOTween.Rewind(_useTarget ? _target : _id, _includeDelay);
        }
    }
}
