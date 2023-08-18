using CBA.Animations.Listeners;

namespace CBA.Animations.Events
{
    public class OnAnimationStepComplete : AnimationEventListener
    {
        protected override void AddListener()
        {
            _animation.Animation.onStepComplete += Invoke;
        }
        protected override void RemoveListener()
        {
            _animation.Animation.onStepComplete -= Invoke;
        }
    }
}
