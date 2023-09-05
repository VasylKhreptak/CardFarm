using System;
using Providers.Graphics;
using UniRx;
using UnityEngine;
using Zenject;

namespace Runtime.Observers
{
    public class CameraCenterObserver : MonoBehaviour
    {
        [Header("Preferences")]
        [SerializeField] private LayerMask _floorLayerMask;
        [SerializeField] private float _updateInterval = 1 / 10f;

        private IDisposable _updateSubscription;

        private Vector3ReactiveProperty _center = new Vector3ReactiveProperty();

        public IReadOnlyReactiveProperty<Vector3> Center => _center;

        private Camera _camera;

        [Inject]
        private void Constructor(CameraProvider cameraProvider)
        {
            _camera = cameraProvider.Value;
        }

        #region MonoBehaviour

        private void OnEnable()
        {
            StartUpdating();
        }

        private void OnDisable()
        {
            StopUpdating();
        }

        #endregion

        private void StartUpdating()
        {
            StopUpdating();

            _updateSubscription = Observable
                .Interval(TimeSpan.FromSeconds(_updateInterval))
                .DoOnSubscribe(OnIntervalTick)
                .Subscribe(_ => OnIntervalTick());
        }

        private void StopUpdating()
        {
            _updateSubscription?.Dispose();
        }

        private void OnIntervalTick()
        {
            UpdateCameraCenter();
        }

        private void UpdateCameraCenter()
        {
            RaycastFloor(out RaycastHit hit);
            _center.Value = hit.point;
        }

        private bool RaycastFloor(out RaycastHit hit)
        {
            Transform cameraTransform = _camera.transform;
            Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
            return UnityEngine.Physics.Raycast(ray, out hit, float.MaxValue, _floorLayerMask);
        }
    }
}
