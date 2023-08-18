using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Action = CBA.Actions.Core.Action;

namespace CBA.Actions.General
{
    public class ActionGroup : Core.Action
    {
        [Header("References")]
        [SerializeField] private Action[] _actions;

        #region MonoBehaviour

        private void OnValidate()
        {
            if (_actions == null || _actions.Length == 0)
            {
                List<Action> actions = transform.GetComponents<Action>().ToList();
                actions.Remove(this);
                _actions = actions.ToArray();
            }
        }

        #endregion

        public override void Do()
        {
            foreach (var action in _actions)
            {
                action.Do();
            }
        }
    }
}
