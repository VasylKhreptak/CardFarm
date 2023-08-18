using CBA.Animations.Actions.Core;

namespace CBA.Animations.Actions.Play
{
    public class PlayFromEndImmediateAnimation : AnimationAction
    {
        public override void Do()
        {
            _animation.PlayFromEndImmediate();
        }
    }
}
