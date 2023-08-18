using CBA.Events.Core;
using NaughtyAttributes;

namespace CBA.Events.General
{
    public class ManualMonoEvent : MonoEvent
    {
        [Button("Invoke")]
        public new void Invoke() => base.Invoke();
    }
}
