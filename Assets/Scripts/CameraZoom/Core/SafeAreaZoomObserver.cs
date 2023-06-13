using System;
using CameraMove.Core;
using Providers;
using UniRx;
using UnityEngine;
using Zenject;

namespace CameraZoom.Core
{
    public class SafeAreaZoomObserver : MonoBehaviour
    {
        [Header("Preferences")]
        [SerializeField] private float _zoomSmoothSpeed = 10f;

        private IDisposable _touchCountDisposable;
        private IDisposable _zoomUpdateSubscription;
        private IDisposable _dragSubscription;

        private FloatReactiveProperty _zoom = new FloatReactiveProperty();
        private FloatReactiveProperty _smoothedZoom = new FloatReactiveProperty();

        private float _lastZoomDistance;

        public IReadOnlyReactiveProperty<float> Zoom => _zoom;
        public IReadOnlyReactiveProperty<float> SmoothedZoom => _smoothedZoom;

        private SafeAreaProvider _safeAreaProvider;
        private SafeAreaDragObserver _dragObserver;

        [Inject]
        private void Constructor(SafeAreaProvider safeAreaProvider,
            SafeAreaDragObserver dragObserver)
        {
            _safeAreaProvider = safeAreaProvider;
            _dragObserver = dragObserver;
        }

        #region MonoBehaviour

        private void OnEnable()
        {
            StartListeningTouchCount();
            StartObservingDrag();
        }

        private void OnDisable()
        {
            ClearSubscriptions();
        }

        private void Update()
        {
            _smoothedZoom.Value = Mathf.Lerp(_smoothedZoom.Value, _zoom.Value, _zoomSmoothSpeed * Time.deltaTime);
        }

        #endregion

        private void ClearSubscriptions()
        {
            StopListeningTouchCount();
            StopUpdatingZoom();
            StopObservingDrag();
        }

        private void StartListeningTouchCount()
        {
            _touchCountDisposable?.Dispose();
            _touchCountDisposable = _safeAreaProvider.Value.TouchCounter.TouchCount.Subscribe(touchCount =>
            {
                if (touchCount == 2)
                {
                    StartUpdatingZoom();
                }
                else
                {
                    StopUpdatingZoom();
                }
            });
        }

        private void StopListeningTouchCount()
        {
            _touchCountDisposable?.Dispose();
        }

        private void StartUpdatingZoom()
        {
            StopUpdatingZoom();

            float GetTouchDistance() => Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);

            _zoomUpdateSubscription = Observable
                .EveryUpdate()
                .DoOnSubscribe(() => _lastZoomDistance = GetTouchDistance())
                .Subscribe(_ =>
                {
                    float currentDistance = GetTouchDistance();
                    float delta = currentDistance - _lastZoomDistance;
                    _lastZoomDistance = currentDistance;
                    _zoom.Value = delta;
                });
        }

        private void StopUpdatingZoom()
        {
            _zoomUpdateSubscription?.Dispose();
            _zoom.Value = 0;
        }

        private void StartObservingDrag()
        {
            StopObservingDrag();

            _dragSubscription = _dragObserver.Delta.Subscribe(delta =>
            {
                _zoom.Value = 0;
                _smoothedZoom.Value = 0;
            });
        }

        private void StopObservingDrag()
        {
            _dragSubscription?.Dispose();
        }
    }
}
