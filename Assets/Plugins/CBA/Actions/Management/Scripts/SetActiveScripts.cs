using CBA.Actions.Management.Scripts.Core;
using UnityEngine;

namespace CBA.Actions.Management.Scripts
{
    public class SetActiveScripts : ScriptsAction
    {
        [Header("Preferences")]
        [SerializeField] private bool _enabled;

        public override void Do()
        {
            foreach (var script in _scripts)
            {
                script.enabled = _enabled;
            }
        }
    }
}
