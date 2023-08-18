using CBA.Animations.Actions.Core;

namespace CBA.Animations.Actions.Play
{
    public class PlayFromEndAnimation : AnimationAction
    {
        public override void Do()
        {
            _animation.PlayFromEnd();
        }
    }
}
