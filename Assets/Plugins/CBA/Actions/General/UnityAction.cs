using UnityEngine;
using UnityEngine.Events;
using Action = CBA.Actions.Core.Action;

namespace CBA.Actions.General
{
    public class UnityAction : Action
    {
        [SerializeField] private UnityEvent _action;

        public override void Do()
        {
            _action.Invoke();
        }
    }
}
