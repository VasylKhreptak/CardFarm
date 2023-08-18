using CBA.Actions.Core;
using DG.Tweening;

namespace CBA.Animations.Actions.Global
{
    public class PauseAllAnimations : Action
    {
        public override void Do()
        {
            DOTween.PauseAll();
        }
    }
}
