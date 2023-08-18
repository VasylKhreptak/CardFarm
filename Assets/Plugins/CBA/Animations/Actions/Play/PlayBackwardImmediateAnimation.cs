using CBA.Animations.Actions.Core;

namespace CBA.Animations.Actions.Play
{
    public class PlayBackwardImmediateAnimation : AnimationAction
    {
        public override void Do()
        {
            _animation.PlayBackwardImmediate();
        }
    }
}
