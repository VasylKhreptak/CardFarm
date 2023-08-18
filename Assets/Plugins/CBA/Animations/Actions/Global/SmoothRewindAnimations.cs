using CBA.Animations.Actions.Global.Core;
using DG.Tweening;

namespace CBA.Animations.Actions.Global
{
    public class SmoothRewindAnimations : TargetOrientedAnimationAction
    {
        public override void Do()
        {
            DOTween.SmoothRewind(_useTarget ? _target : _id);
        }
    }
}
