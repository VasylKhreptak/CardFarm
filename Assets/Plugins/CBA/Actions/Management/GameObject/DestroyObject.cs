using CBA.Actions.Management.GameObject.Core;

namespace CBA.Actions.Management.GameObject
{
    public class DestroyObject : GameObjectAction
    {
        public override void Do()
        {
            Destroy(_gameObject);
        }
    }
}
