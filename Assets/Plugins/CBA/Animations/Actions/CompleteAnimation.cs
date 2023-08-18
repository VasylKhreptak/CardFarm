using CBA.Animations.Actions.Core;
using DG.Tweening;

namespace CBA.Animations.Actions
{
    public class CompleteAnimation : AnimationAction
    {
        public override void Do()
        {
            _animation.Animation.Complete();
        }
    }
}
