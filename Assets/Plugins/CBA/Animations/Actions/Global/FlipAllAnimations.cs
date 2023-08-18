using CBA.Actions.Core;
using DG.Tweening;

namespace CBA.Animations.Actions.Global
{
    public class FlipAllAnimations : Action
    {
        public override void Do()
        {
            DOTween.FlipAll();
        }
    }
}
