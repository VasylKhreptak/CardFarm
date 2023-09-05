using System;
using Extensions;
using Runtime.Observers;
using UniRx;
using UnityEngine;
using Zenject;
using Color = CBA.Extensions.Color;

namespace Runtime.CameraCenterDependencyBehaviour.Core
{
    public abstract class CameraCenterSphereDependentObject : MonoBehaviour
    {
        [Header("Sphere Preferences")]
        [SerializeField] private float _minRange;
        [SerializeField] private float _maxRange;
        [SerializeField] private AnimationCurve _curve;

        private IDisposable _cameraCenterSubscription;

        private CameraCenterObserver _cameraCenterObserver;

        [Inject]
        private void Constructor(CameraCenterObserver cameraCenterObserver)
        {
            _cameraCenterObserver = cameraCenterObserver;
        }

        #region MonoBehaviour

        private void OnEnable()
        {
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
        }

        #endregion

        private void StartObserving()
        {
            StopObserving();

            _cameraCenterSubscription = _cameraCenterObserver.Center.Subscribe(OnCameraCenterChanged);
        }

        private void StopObserving()
        {
            _cameraCenterSubscription?.Dispose();
        }

        private void OnCameraCenterChanged(Vector3 center)
        {
            float distance = Vector3.Distance(transform.position, center);

            OnEvaluatedDistance(_curve.Evaluate(_minRange, _maxRange, distance, 1f, 0f));
        }

        public abstract void OnEvaluatedDistance(float value);

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.WithAlpha(UnityEngine.Color.red, 0.2f);

            Gizmos.DrawSphere(transform.position, _minRange);
            Gizmos.DrawSphere(transform.position, _maxRange);
        }
    }
}
