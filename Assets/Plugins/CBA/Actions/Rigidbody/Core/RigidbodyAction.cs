using UnityEngine;
using Action = CBA.Actions.Core.Action;

namespace CBA.Actions.Rigidbody.Core
{
    public abstract class RigidbodyAction : Action
    {
        [Header("References")]
        [SerializeField] protected UnityEngine.Rigidbody _rigidbody;

        #region MonoBehaviour

        private void OnValidate()
        {
            _rigidbody ??= GetComponent<UnityEngine.Rigidbody>();
        }

        #endregion
    }
}
