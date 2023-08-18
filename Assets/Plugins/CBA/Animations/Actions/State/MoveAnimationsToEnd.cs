using CBA.Animations.Actions.Core;

namespace CBA.Animations.Actions.State
{
    public class MoveAnimationsToEnd : AnimationsAction
    {
        public override void Do()
        {
            foreach (var animation in _animations)
            {
                animation.MoveToEndState();
            }
        }
    }
}
