using System;
using CameraZoom.Core;
using Providers;
using UniRx;
using UnityEngine;
using Zenject;

namespace CameraZoom
{
    public class CameraZoomLogic : MonoBehaviour
    {
        [Header("Preferences")]
        [SerializeField] private LayerMask _floorLayerMask;
        [SerializeField] private float _zoomSpeed = 1f;
        [SerializeField] private float _minDistance = 5f;
        [SerializeField] private float _maxDistance = 35f;

        private IDisposable _zoomSubscription;

        private Transform _cameraTransform;
        private SafeAreaZoomObserver _safeAreaZoomObserver;

        [Inject]
        private void Constructor(CameraProvider cameraProvider, SafeAreaZoomObserver safeAreaZoomObserver)
        {
            _cameraTransform = cameraProvider.Value.transform;
            _safeAreaZoomObserver = safeAreaZoomObserver;
        }

        #region MonoBehaviour

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

            _zoomSubscription = _safeAreaZoomObserver.SmoothedZoom.Subscribe(Zoom);
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
                float newCameraDistance = cameraDistance - delta * _zoomSpeed;
                newCameraDistance = Mathf.Clamp(newCameraDistance, _minDistance, _maxDistance);
                Vector3 newCameraPosition = hit.point - _cameraTransform.forward * newCameraDistance;
                _cameraTransform.position = newCameraPosition;
            }
        }

        private bool RaycastFloor(out RaycastHit hit)
        {
            Ray ray = new Ray(_cameraTransform.position, _cameraTransform.forward);
            return Physics.Raycast(ray, out hit, _maxDistance, _floorLayerMask);
        }
    }
}
