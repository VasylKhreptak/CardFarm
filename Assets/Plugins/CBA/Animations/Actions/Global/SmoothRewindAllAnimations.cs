using CBA.Actions.Core;
using DG.Tweening;

namespace CBA.Animations.Actions.Global
{
    public class SmoothRewindAllAnimations : Action
    {
        public override void Do()
        {
            DOTween.SmoothRewindAll();
        }
    }
}
