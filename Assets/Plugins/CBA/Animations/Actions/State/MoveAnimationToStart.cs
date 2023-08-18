using CBA.Animations.Actions.Core;

namespace CBA.Animations.Actions.State
{
    public class MoveAnimationToStart : AnimationAction
    {
        public override void Do()
        {
            _animation.MoveToStartState();
        }
    }
}
