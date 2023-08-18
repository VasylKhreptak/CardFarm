using CBA.Actions.Core;
using DG.Tweening;

namespace CBA.Animations.Actions.Global
{
    public class TogglePauseAllAnimations : Action
    {
        public override void Do()
        {
            DOTween.TogglePauseAll();
        }
    }
}
