using CBA.Actions.Core;
using UnityEngine;

namespace CBA.Actions.Management.Scripts.Core
{
    public abstract class ScriptsAction : Action
    {
        [Header("References")]
        [SerializeField] protected Behaviour[] _scripts;
    }
}
