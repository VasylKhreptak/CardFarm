using CBA.Animations.Actions.Core;

namespace CBA.Animations.Actions.Play
{
    public class PlayForwardAnimation : AnimationAction
    {
        public override void Do()
        {
            _animation.PlayForward();
        }
    }
}
