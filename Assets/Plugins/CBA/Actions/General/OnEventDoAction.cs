using CBA.EventListeners;
using NaughtyAttributes;
using UnityEngine;
using Action = CBA.Actions.Core.Action;

namespace CBA.Actions.General
{
    public class OnEventDoAction : MonoEventListener
    {
        [Header("References")]
        [Required, SerializeField] private Action _action;

        #region MonoBehaviour

        protected override void OnValidate()
        {
            base.OnValidate();

            _action ??= GetComponent<Action>();
        }

        protected override void OnEventFired()
        {
            _action.Do();
        }

        #endregion
    }
}
