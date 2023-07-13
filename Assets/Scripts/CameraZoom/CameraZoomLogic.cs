using System;
using CameraZoom.Core;
using UniRx;
using UnityEngine;
using Zenject;

namespace CameraZoom
{
    public class CameraZoomLogic : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _transform;

        [Header("Preferences")]
        [SerializeField] private LayerMask _floorLayerMask;
        [SerializeField] private float _zoomSpeed = 1f;
        [SerializeField] private float _minDistance = 5f;
        [SerializeField] private float _maxDistance = 35f;

        private IDisposable _zoomSubscription;

        private FloatReactiveProperty _cameraDistance = new FloatReactiveProperty();

        public IReadOnlyReactiveProperty<float> CameraDistance => _cameraDistance;

        private ZoomObserver _safeAreaZoomObserver;

        [Inject]
        private void Constructor(ZoomObserver safeAreaZoomObserver)
        {
            _safeAreaZoomObserver = safeAreaZoomObserver;
        }

        #region MonoBehaviour

        private void Awake()
        {
            if (RaycastFloor(out RaycastHit hit))
            {
                _cameraDistance.Value = hit.distance;
            }
        }

        private void OnEnable()
        {
            StartZooming();
        }

        private void OnDisable()
        {
            StopZooming();
        }

        #endregion

        private void StartZooming()
        {
            StopZooming();

            _zoomSubscription = _safeAreaZoomObserver.Zoom.Subscribe(Zoom);
        }

        private void StopZooming()
        {
            _zoomSubscription?.Dispose();
        }

        private void Zoom(float delta)
        {
            if (RaycastFloor(out RaycastHit hit))
            {
                float cameraDistance = hit.distance;
                _cameraDistance.Value = cameraDistance;
                float newCameraDistance = cameraDistance - delta * _zoomSpeed;
                newCameraDistance = Mathf.Clamp(newCameraDistance, _minDistance, _maxDistance);
                Vector3 newCameraPosition = hit.point - _transform.forward * newCameraDistance;
                _transform.position = newCameraPosition;
            }
        }

        private bool RaycastFloor(out RaycastHit hit)
        {
            Ray ray = new Ray(_transform.position, _transform.forward);
            return UnityEngine.Physics.Raycast(ray, out hit, _maxDistance, _floorLayerMask);
        }
    }
}
