using CBA.EventListeners;

namespace CBA.Events.General
{
    public class EventRepeater : MonoEventListener
    {
        protected override void OnEventFired()
        {
            Invoke();
        }
    }
}
