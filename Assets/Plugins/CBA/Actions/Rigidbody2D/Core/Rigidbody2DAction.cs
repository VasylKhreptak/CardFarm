using UnityEngine;
using Action = CBA.Actions.Core.Action;

namespace CBA.Actions.Rigidbody2D.Core
{
    public abstract class Rigidbody2DAction : Action
    {
        [Header("References")]
        [SerializeField] protected UnityEngine.Rigidbody2D _rigidbody2D;

        #region MonoBehaviour

        private void OnValidate()
        {
            _rigidbody2D ??= GetComponent<UnityEngine.Rigidbody2D>();
        }

        #endregion
    }
}
