namespace CBA.Events.Core
{
    public class MonoEvent : UnityEngine.MonoBehaviour
    {
        public event System.Action onMonoCall;

        protected void Invoke() => onMonoCall?.Invoke();
    }
}
