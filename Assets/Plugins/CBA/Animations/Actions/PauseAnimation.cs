using CBA.Animations.Actions.Core;
using DG.Tweening;

namespace CBA.Animations.Actions
{
    public class PauseAnimation : AnimationAction
    {
        public override void Do()
        {
            _animation.Animation.Pause();
        }
    }
}
