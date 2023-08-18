using CBA.Animations.Core;
using UnityEngine;
using UnityEngine.Serialization;
using Animation = CBA.Animations.Core.Animation;

namespace CBA.Animations.Physics.Rigidbody2D.Core
{
    public abstract class Rigidbody2DAnimation : Animation
    {
        [Header("References")]
        [SerializeField] protected UnityEngine.Transform _transform;
        [SerializeField] protected UnityEngine.Rigidbody2D _rigidbody2D;

        #region MonoBehaviour

        private void OnValidate()
        {
            _transform ??= GetComponent<UnityEngine.Transform>();
            _rigidbody2D ??= GetComponent<UnityEngine.Rigidbody2D>();
        }

        #endregion
    }
}
