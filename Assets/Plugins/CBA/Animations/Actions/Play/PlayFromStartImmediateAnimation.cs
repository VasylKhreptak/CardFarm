using CBA.Animations.Actions.Core;

namespace CBA.Animations.Actions.Play
{
    public class PlayFromStartImmediateAnimation : AnimationAction
    {
        public override void Do()
        {
            _animation.PlayFromStartImmediate();
        }
    }
}
