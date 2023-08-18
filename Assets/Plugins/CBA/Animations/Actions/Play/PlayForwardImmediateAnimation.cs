using CBA.Animations.Actions.Core;

namespace CBA.Animations.Actions.Play
{
    public class PlayForwardImmediateAnimation : AnimationAction
    {
        public override void Do()
        {
            _animation.PlayForwardImmediate();
        }
    }
}
