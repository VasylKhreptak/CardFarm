using CBA.Actions.Management.GameObjects.Core;
using UnityEngine;

namespace CBA.Actions.Management.GameObjects
{
    public class SetActiveObjects : GameObjectsAction
    {
        [Header("Preferences")]
        [SerializeField] private bool _active;
        
        public override void Do()
        {
            foreach (var go in _gameObjects)
            {
                go.SetActive(_active);
            }
        }
    }
}
