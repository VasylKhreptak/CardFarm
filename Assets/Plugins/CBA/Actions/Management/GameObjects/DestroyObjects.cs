using CBA.Actions.Management.GameObjects.Core;

namespace CBA.Actions.Management.GameObjects
{
    public class DestroyObjects : GameObjectsAction
    {
        public override void Do()
        {
            foreach (var go in _gameObjects)
            {
                Destroy(go);
            }
        }
    }
}
