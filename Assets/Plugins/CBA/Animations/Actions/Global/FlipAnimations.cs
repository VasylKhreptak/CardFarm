using CBA.Animations.Actions.Global.Core;
using DG.Tweening;

namespace CBA.Animations.Actions.Global
{
    public class FlipAnimations : TargetOrientedAnimationAction
    {
        public override void Do()
        {
            DOTween.Flip(_useTarget ? _target : _id);
        }
    }
}
