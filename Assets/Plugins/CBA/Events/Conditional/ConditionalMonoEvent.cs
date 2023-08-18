using CBA.EventListeners;
using CBA.Predicates.Core;
using UnityEngine;

namespace CBA.Events.Conditional
{
    public class ConditionalMonoEvent : MonoEventListener
    {
        [Header("References")]
        [SerializeField] private Predicate _predicate;

        #region MonoBehaviour

        protected override void OnValidate()
        {
            base.OnValidate();

            _predicate ??= GetComponent<Predicate>();
        }

        protected override void OnEventFired()
        {
            if (_predicate.Result())
            {
                Invoke();
            }
        }

        #endregion
    }
}
