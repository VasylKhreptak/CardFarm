using CBA.Actions.Management.Scripts.Core;

namespace CBA.Actions.Management.Scripts
{
    public class DestroyScripts : ScriptsAction
    {
        public override void Do()
        {
            foreach (var script in _scripts)
            {
                Destroy(script);
            }
        }
    }
}
