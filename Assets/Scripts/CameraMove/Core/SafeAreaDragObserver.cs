using System;
using Providers;
using UniRx;
using UnityEngine;
using Zenject;

namespace CameraMove.Core
{
    public class SafeAreaDragObserver : MonoBehaviour
    {
        [Header("Preferences")]
        [SerializeField] private float _smoothSpeed = 10f;
        [SerializeField] private LayerMask _floorLayerMask;

        private IDisposable _touchCountSubscription;
        private IDisposable _updateSubscription;

        private Vector2ReactiveProperty _delta = new Vector2ReactiveProperty();
        private Vector2ReactiveProperty _smoothedDelta = new Vector2ReactiveProperty();

        private Vector3 _previousFloorPoint;

        public IReadOnlyReactiveProperty<Vector2> Delta => _delta;
        public IReadOnlyReactiveProperty<Vector2> SmoothedDelta => _smoothedDelta;

        private SafeAreaProvider _safeAreaProvider;
        private Camera _camera;

        [Inject]
        private void Constructor(SafeAreaProvider safeAreaProvider,
            CameraProvider cameraProvider)
        {
            _safeAreaProvider = safeAreaProvider;
            _camera = cameraProvider.Value;
        }

        #region MonoBehaviour

        private void OnEnable()
        {
            StartObservingTouchCount();
        }

        private void OnDisable()
        {
            StopObserving();
        }

        private void Update()
        {
            SmoothDelta();
        }

        #endregion

        private void SmoothDelta()
        {
            _smoothedDelta.Value = Vector2.Lerp(_smoothedDelta.Value, _delta.Value, _smoothSpeed * Time.deltaTime);
        }

        private void StartObservingTouchCount()
        {
            StopObservingTouchCount();

            _touchCountSubscription = _safeAreaProvider.Value.TouchCounter.TouchCount.Subscribe(OnTouchCountUpdated);
        }

        private void StopObservingTouchCount()
        {
            _touchCountSubscription?.Dispose();
        }

        private void StopObserving()
        {
            StopObservingTouchCount();
        }

        private void OnTouchCountUpdated(int touchCount)
        {
            if (touchCount == 1)
            {
                StartUpdatingDrag();
            }
            else
            {
                StopUpdatingDrag();
            }
        }

        private void StartUpdatingDrag()
        {
            StopUpdatingDrag();

            UpdatePreviousFloorPoint();
            _delta.Value = Vector2.zero;

            _updateSubscription = Observable.EveryUpdate().Subscribe(_ => UpdateDrag());
        }

        private void StopUpdatingDrag()
        {
            _updateSubscription?.Dispose();

            _delta.Value = Vector2.zero;
        }

        private void UpdateDrag()
        {
            Vector3 floorPoint = Vector3.zero;

            if (RaycastFloor(Input.GetTouch(0).position, out RaycastHit hitInfo))
            {
                floorPoint = hitInfo.point;
            }

            Vector3 difference = floorPoint - _previousFloorPoint;
            
            _delta.Value = new Vector2(difference.x, difference.z);
            
            _previousFloorPoint = floorPoint;
        }

        private void UpdatePreviousFloorPoint()
        {
            if (RaycastFloor(Input.GetTouch(0).position, out RaycastHit hitInfo))
            {
                _previousFloorPoint = hitInfo.point;
            }
        }

        private bool RaycastFloor(Vector2 screenPosition, out RaycastHit hit)
        {
            var ray = _camera.ScreenPointToRay(screenPosition);
            return Physics.Raycast(ray, out hit, Mathf.Infinity, _floorLayerMask);
        }
    }
}
