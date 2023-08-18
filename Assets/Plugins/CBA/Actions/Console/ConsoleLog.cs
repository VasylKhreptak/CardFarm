using CBA.Actions.Core;
using UnityEngine;

namespace CBA.Actions.Console
{
    public class ConsoleLog : Action
    {
        [Header("Preferences")]
        [SerializeField] private string _message;

        public override void Do()
        {
            Debug.Log(_message);
        }
    }
}
