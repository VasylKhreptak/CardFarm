using CBA.Actions.Core;
using NaughtyAttributes;
using UnityEngine;

namespace CBA.Actions.Management.Scene.Core
{
    public abstract class SceneAction : Action
    {
        [Header("References")]
        [Scene, SerializeField] protected string _scene;
    }
}
