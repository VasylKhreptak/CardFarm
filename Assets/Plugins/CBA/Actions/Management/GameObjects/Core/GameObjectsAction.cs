using CBA.Actions.Core;
using UnityEngine;

namespace CBA.Actions.Management.GameObjects.Core
{
    public abstract class GameObjectsAction : Action
    {
        [Header("References")]
        [SerializeField] protected UnityEngine.GameObject[] _gameObjects;
    }
}
