using CBA.Animations.Transform.Look.Core;
using DG.Tweening;

namespace CBA.Animations.Transform.Look
{
    public class DynamicLookAtAnimation : LookAtAnimation
    {
        public override Tween CreateForwardAnimation()
        {
            return _transform.DODynamicLookAt(_endTarget.position, _duration, _axisConstraint, _up);
        }

        public override Tween CreateBackwardAnimation()
        {
            return _transform.DODynamicLookAt(_starTarget.position, _duration, _axisConstraint, _up);
        }
    }
}
