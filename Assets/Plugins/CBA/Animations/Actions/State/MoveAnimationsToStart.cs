using CBA.Animations.Actions.Core;

namespace CBA.Animations.Actions.State
{
    public class MoveAnimationsToStart : AnimationsAction
    {
        public override void Do()
        {
            foreach (var animation in _animations)
            {
                animation.MoveToStartState();
            }
        }
    }
}
