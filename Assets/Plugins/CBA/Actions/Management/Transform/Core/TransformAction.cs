using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace CBA.Actions.Management.Transform.Core
{
    public abstract class TransformAction : Actions.Core.Action
    {
        [Header("References")]
        [Required, SerializeField] protected UnityEngine.Transform _transform;

        [Header("Constraint")]
        [SerializeField] private AxisConstraint _axisConstraint = AxisConstraint.None;

        protected Vector3Int AllowedAxes => Extensions.AxisConstraint.AllowedAxes(_axisConstraint);

        #region MonoBehaviour

        protected virtual void OnValidate()
        {
            _transform ??= GetComponent<UnityEngine.Transform>();
        }

        #endregion
    }
}
