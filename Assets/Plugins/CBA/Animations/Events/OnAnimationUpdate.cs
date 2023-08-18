using CBA.Animations.Listeners;

namespace CBA.Animations.Events
{
    public class OnAnimationUpdate : AnimationEventListener
    {
        protected override void AddListener()
        {
            _animation.Animation.onUpdate += Invoke;
        }
        protected override void RemoveListener()
        {
            _animation.Animation.onUpdate -= Invoke;
        }
    }
}
