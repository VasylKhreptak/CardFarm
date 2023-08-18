using CBA.Animations.Listeners;

namespace CBA.Animations.Events
{
    public class OnAnimationPause : AnimationEventListener
    {
        protected override void AddListener()
        {
            _animation.Animation.onPause += Invoke;
        }
        protected override void RemoveListener()
        {
            _animation.Animation.onPause -= Invoke;
        }
    }
}
