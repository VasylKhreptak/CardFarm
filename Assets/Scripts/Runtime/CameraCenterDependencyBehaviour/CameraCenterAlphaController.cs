using System;
using Runtime.CameraCenterDependencyBehaviour.Core;
using UnityEngine;

namespace Runtime.CameraCenterDependencyBehaviour
{
    public class CameraCenterAlphaController : CameraCenterSphereDependentObject
    {
        [Header("References")]
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header("Preferences")]
        [SerializeField] private float _minAlpha;
        [SerializeField] private float _maxAlpha;

        #region MonoBehaviour

        private void OnValidate()
        {
            _canvasGroup ??= GetComponent<CanvasGroup>();
        }

        #endregion

        public override void OnEvaluatedDistance(float value)
        {
            _canvasGroup.alpha = Mathf.Lerp(_minAlpha, _maxAlpha, value);
        }
    }
}
