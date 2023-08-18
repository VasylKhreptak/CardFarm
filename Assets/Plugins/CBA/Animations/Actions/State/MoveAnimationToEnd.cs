using CBA.Animations.Actions.Core;

namespace CBA.Animations.Actions.State
{
    public class MoveAnimationToEnd : AnimationAction
    {
        public override void Do()
        {
            _animation.MoveToEndState();
        }
    }
}
