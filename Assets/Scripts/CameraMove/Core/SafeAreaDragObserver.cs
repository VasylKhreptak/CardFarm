using System;
using Providers;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace CameraMove.Core
{
    public class SafeAreaDragObserver : MonoBehaviour
    {
        [Header("Preferences")]
        [SerializeField] private float _smoothSpeed = 10f;
        [SerializeField] private LayerMask _floorLayerMask;

        private IDisposable _dragSubscription;
        private IDisposable _pointerDownSubscription;
        private IDisposable _pointerUpSubscription;

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
            StartObservingPointerDown();
            StartObservingPointerUp();
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

        private void StartObservingPointerDown()
        {
            StopObservingPointerDown();
            _pointerDownSubscription = _safeAreaProvider.Value.Behaviour.OnPointerDownAsObservable().Subscribe(OnPointerDown);
        }

        private void StopObservingPointerDown()
        {
            _pointerDownSubscription?.Dispose();
        }

        private void StartObservingPointerUp()
        {
            StopObservingPointerUp();
            _pointerUpSubscription = _safeAreaProvider.Value.Behaviour.OnPointerUpAsObservable().Subscribe(OnPointerUp);
        }

        private void StopObservingPointerUp()
        {
            _pointerUpSubscription?.Dispose();
        }

        private void SmoothDelta()
        {
            _smoothedDelta.Value = Vector2.Lerp(_smoothedDelta.Value, _delta.Value, _smoothSpeed * Time.deltaTime);
        }

        private void StopObserving()
        {
            StopObservingPointerDown();
            StopObservingPointerUp();
            StopObservingDrag();
        }

        private void OnPointerDown(PointerEventData pointerData)
        {
            if (pointerData.pointerId == 0)
            {
                UpdatePreviousFloorPoint(pointerData.position);
                StartObservingDrag();
            }
            else
            {
                StopObservingDrag();
            }
        }

        private void OnPointerUp(PointerEventData pointerData)
        {
            if (pointerData.pointerId == 0)
            {
                UpdatePreviousFloorPoint(pointerData.position);
            }

            StopObservingDrag();
        }

        private void StartObservingDrag()
        {
            _delta.Value = Vector2.zero;

            StopObservingDrag();
            _dragSubscription = _safeAreaProvider.Value.Behaviour.OnDragAsObservable().Subscribe(dragData =>
            {
                Vector2 dragPosition = dragData.position;

                if (RaycastFloor(dragPosition, out var hit) == false) return;

                Vector3 floorPoint = hit.point;

                Vector3 delta = floorPoint - _previousFloorPoint;

                _delta.Value = new Vector2(delta.x, delta.z);

                UpdatePreviousFloorPoint(dragPosition);
            });
        }

        private void StopObservingDrag()
        {
            _dragSubscription?.Dispose();
            _delta.Value = Vector2.zero;
        }


        private void UpdatePreviousFloorPoint(Vector2 screenPosition)
        {
            if (RaycastFloor(screenPosition, out var hit))
            {
                _previousFloorPoint = hit.point;
            }
        }

        private bool RaycastFloor(Vector2 screenPosition, out RaycastHit hit)
        {
            var ray = _camera.ScreenPointToRay(screenPosition);
            return Physics.Raycast(ray, out hit, Mathf.Infinity, _floorLayerMask);
        }
    }
}
