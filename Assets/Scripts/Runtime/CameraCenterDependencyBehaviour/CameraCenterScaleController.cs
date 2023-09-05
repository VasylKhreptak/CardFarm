using Runtime.CameraCenterDependencyBehaviour.Core;
using UnityEngine;

namespace Runtime.CameraCenterDependencyBehaviour
{
    public class CameraCenterScaleController : CameraCenterSphereDependentObject
    {
        [Header("Preferences")]
        [SerializeField] private Vector3 _minScale;
        [SerializeField] private Vector3 _maxScale;

        public override void OnEvaluatedDistance(float value)
        {
            transform.localScale = Vector3.Lerp(_minScale, _maxScale, value);
        }
    }
}
