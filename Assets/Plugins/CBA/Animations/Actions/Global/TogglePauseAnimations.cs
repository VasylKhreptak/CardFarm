using CBA.Animations.Actions.Global.Core;
using DG.Tweening;

namespace CBA.Animations.Actions.Global
{
    public class TogglePauseAnimations : TargetOrientedAnimationAction
    {
        public override void Do()
        {
            DOTween.TogglePause(_useTarget ? _target : _id);
        }
    }
}
