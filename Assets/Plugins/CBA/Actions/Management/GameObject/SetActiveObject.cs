using CBA.Actions.Management.GameObject.Core;
using UnityEngine;

namespace CBA.Actions.Management.GameObject
{
    public class SetActiveObject : GameObjectAction
    {
        [Header("Preferences")]
        [SerializeField] private bool _active;
        
        public override void Do()
        {
            _gameObject.SetActive(_active);
        }
    }
}
