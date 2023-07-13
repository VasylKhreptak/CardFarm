using System;
using Providers;
using UniRx;
using UnityEngine;
using Zenject;

namespace CameraZoom.Core
{
    public class ZoomObserver : MonoBehaviour
    {
        private IDisposable _touchCountDisposable;
        private IDisposable _zoomUpdateSubscription;
        private IDisposable _dragSubscription;

        private FloatReactiveProperty _zoom = new FloatReactiveProperty();

        private float _lastZoomDistance;

        public IReadOnlyReactiveProperty<float> Zoom => _zoom;

        #region MonoBehaviour

        private void OnEnable()
        {
            // StartListeningTouchCount();
        }

        private void OnDisable()
        {
            ClearSubscriptions();
        }

        #endregion

        private void ClearSubscriptions()
        {
            StopListeningTouchCount();
            StopUpdatingZoom();
        }

        // private void StartListeningTouchCount()
        // {
        //     _touchCountDisposable?.Dispose();
        //     _touchCountDisposable = _safeAreaProvider.Value.TouchCounter.TouchCount.Subscribe(touchCount =>
        //     {
        //         if (touchCount == 2)
        //         {
        //             StartUpdatingZoom();
        //         }
        //         else
        //         {
        //             StopUpdatingZoom();
        //         }
        //     });
        // }

        private void StopListeningTouchCount()
        {
            _touchCountDisposable?.Dispose();
        }

        private void StartUpdatingZoom()
        {
            StopUpdatingZoom();

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

        private float GetTouchDistance()
        {
            if (Input.touchCount < 2)
            {
                return _lastZoomDistance;
            }

            return Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
        }
    }
}
