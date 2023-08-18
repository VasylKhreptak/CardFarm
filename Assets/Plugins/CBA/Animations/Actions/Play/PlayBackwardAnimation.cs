using CBA.Animations.Actions.Core;

namespace CBA.Animations.Actions.Play
{
    public class PlayBackwardAnimation : AnimationAction
    {
        public override void Do()
        {
            _animation.PlayBackward();
        }
    }
}
