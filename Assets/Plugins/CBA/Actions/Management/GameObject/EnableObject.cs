using CBA.Actions.Management.GameObject.Core;

namespace CBA.Actions.Management.GameObject
{
    public class EnableObject : GameObjectAction
    {
        public override void Do()
        {
            _gameObject.SetActive(true);
        }
    }
}
