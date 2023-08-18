using CBA.Animations.Actions.Global.Core;
using DG.Tweening;
using UnityEngine;

namespace CBA.Animations.Actions.Global
{
    public class KillAnimations : TargetOrientedAnimationAction
    {
        [Header("Complete")]
        [SerializeField] private bool _complete;

        public override void Do()
        {
            DOTween.Kill(_useTarget ? _target : _id, _complete);
        }
    }
}
