using CBA.EventListeners;
using UnityEngine;

namespace CBA.Events.Random
{
    public class ProbableMonoEvent : MonoEventListener
    {
        [Header("Preferences")]
        [SerializeField, Range(0f, 1f)] private float _probability;

        protected override void OnEventFired()
        {
            Extensions.Mathf.Probability(_probability, Invoke);
        }
    }
}
