using CBA.Actions.Management.Scripts.Core;

namespace CBA.Actions.Management.Scripts
{
    public class DisableScripts : ScriptsAction
    {
        public override void Do()
        {
            foreach (var script in _scripts)
            {
                script.enabled = false;
            }
        }
    }
}
