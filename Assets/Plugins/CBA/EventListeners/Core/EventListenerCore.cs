using CBA.Events.Core;

namespace CBA.EventListeners.Core
{
    public abstract class EventListenerCore : MonoEvent
    {
        private bool _addedListener;

        protected void TryAddListener()
        {
            if (_addedListener == false)
            {
                AddListener();

                _addedListener = true;
            }
        }

        protected void TryRemoveListener()
        {
            if (_addedListener)
            {
                RemoveListener();

                _addedListener = false;
            }
        }
        
        protected abstract void AddListener();

        protected abstract void RemoveListener();
    }
}
