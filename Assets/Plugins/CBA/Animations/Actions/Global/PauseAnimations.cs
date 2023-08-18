using CBA.Animations.Actions.Global.Core;
using DG.Tweening;

namespace CBA.Animations.Actions.Global
{
    public class PauseAnimations : TargetOrientedAnimationAction
    {
        public override void Do()
        {
            DOTween.Pause(_useTarget ? _target : _id);
        }
    }
}
