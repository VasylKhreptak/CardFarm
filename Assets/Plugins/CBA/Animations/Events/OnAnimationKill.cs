using CBA.Animations.Listeners;

namespace CBA.Animations.Events
{
    public class OnAnimationKill : AnimationEventListener
    {
        protected override void AddListener()
        {
            _animation.Animation.onKill += Invoke;
        }
        protected override void RemoveListener()
        {
            _animation.Animation.onKill -= Invoke;
        }
    }
}
