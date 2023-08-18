using CBA.Actions.Core;
using UnityEngine;

namespace CBA.Actions.General
{
    public class ToggleAction : Action
    {
        [Header("References")]
        [SerializeField] private Action _first;
        [SerializeField] private Action _second;

        private bool _invokedFirst;
        
        public override void Do()
        {
            if (_invokedFirst == false)
            {
                _first.Do();
                _invokedFirst = true;
                
                return;
            }
            
            _second.Do();
            _invokedFirst = false;
        }
    }
}
