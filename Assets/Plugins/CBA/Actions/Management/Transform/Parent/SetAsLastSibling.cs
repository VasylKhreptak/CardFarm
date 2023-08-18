using CBA.Actions.Management.Transform.Core;

namespace CBA.Actions.Management.Transform.Parent
{
    public class SetAsLastSibling : TransformAction
    {
        public override void Do()
        {
            _transform.SetAsLastSibling();
        }
    }
}
