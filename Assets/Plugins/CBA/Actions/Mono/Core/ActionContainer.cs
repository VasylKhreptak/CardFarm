using UnityEngine;
using Action = CBA.Actions.Core.Action;

namespace CBA.Actions.Mono.Core
{
    public class ActionContainer : MonoBehaviour
    {
        [Header("Action")]
        [SerializeField] protected Action _action;

        #region MonoBehaviour

        private void OnValidate()
        {
            _action ??= GetComponent<Action>();
        }

        #endregion
    }
}
