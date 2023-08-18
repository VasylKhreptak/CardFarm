using CBA.Actions.Core;
using CBA.Extensions;
using UnityEngine;

namespace CBA.Actions.General
{
    public class DoActionLooped : Action
    {
        [Header("References")]
        [SerializeField] private Action[] _actions;

        private Action _currentAction;

        public override void Do()
        {
            if (_currentAction == null)
            {
                _currentAction = _actions.First();
                _currentAction.Do();

                return;
            }

            _currentAction = _actions.Next(_currentAction);
            _currentAction.Do();
        }
    }
}
