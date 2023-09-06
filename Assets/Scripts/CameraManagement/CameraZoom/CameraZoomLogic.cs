using System;
using CameraManagement.CameraZoom.Core;
using Providers.Graphics;
using UniRx;
using UnityEngine;
using Zenject;

namespace CameraManagement.CameraZoom
{
    public class CameraZoomLogic : MonoBehaviour
    {
        [Header("Preferences")]
        [SerializeField] private float _zoomSpeed = 1f;
        [SerializeField] private float _minSize = 5f;
        [SerializeField] private float _maxSize = 10f;

        private IDisposable _zoomSubscription;

        public float MinSize => _minSize;
        public float MaxSize => _maxSize;
        
        private ZoomObserver _safeAreaZoomObserver;
        private Camera _camera;

        [Inject]
        private void Constructor(ZoomObserver safeAreaZoomObserver, CameraProvider camera)
        {
            _safeAreaZoomObserver = safeAreaZoomObserver;
            _camera = camera.Value;
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

            _zoomSubscription = _safeAreaZoomObserver.Zoom.Subscribe(Zoom);
        }

        private void StopObserving()
        {
            _zoomSubscription?.Dispose();
        }

        private void Zoom(float delta)
        {
            float newSize = _camera.orthographicSize + delta * _zoomSpeed * Time.deltaTime;

            _camera.orthographicSize = Mathf.Clamp(newSize, _minSize, _maxSize);
        }
    }
}
