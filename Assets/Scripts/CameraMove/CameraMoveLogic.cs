using System;
using CameraMove.Core;
using CameraZoom;
using Providers;
using UniRx;
using UnityEngine;
using Zenject;

namespace CameraMove
{
    public class CameraMoveLogic : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private float _speed;

        private IDisposable _dragSubscription;

        private MapDragObserver _dragObserver;
        private Transform _cameraTransform;
        private CameraZoomLogic _cameraZoomLogic;

        [Inject]
        private void Constructor(MapDragObserver dragObserver,
            CameraProvider cameraProvider,
            CameraZoomLogic cameraZoomLogic)
        {
            _dragObserver = dragObserver;
            _cameraTransform = cameraProvider.Value.transform;
            _cameraZoomLogic = cameraZoomLogic;
        }

        #region MonoBehaviour

        private void OnEnable()
        {
            StartObservingDrag();
        }

        private void OnDisable()
        {
            StopObservingDrag();
        }

        #endregion

        private void StartObservingDrag()
        {
            StopObservingDrag();

            _dragSubscription = _dragObserver.Delta.Subscribe(MoveCamera);
        }

        private void StopObservingDrag()
        {
            _dragSubscription?.Dispose();
        }

        private void MoveCamera(Vector2 dragDelta)
        {
            if (Input.touchCount > 1) return;

            Vector3 moveDirection = new Vector3(-dragDelta.x, 0f, -dragDelta.y);

            _cameraTransform.position += moveDirection * _speed * Time.deltaTime * _cameraZoomLogic.CameraDistance.Value;
        }
    }
}
