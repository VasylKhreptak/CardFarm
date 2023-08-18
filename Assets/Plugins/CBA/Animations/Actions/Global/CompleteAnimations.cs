using CBA.Animations.Actions.Global.Core;
using DG.Tweening;
using UnityEngine;

namespace CBA.Animations.Actions.Global
{
    public class CompleteAnimations : TargetOrientedAnimationAction
    {
        [Header("Callback")]
        [SerializeField] private bool _withCallback;

        public override void Do()
        {
            DOTween.Complete(_useTarget ? _target : _id, _withCallback);
        }
    }
}
