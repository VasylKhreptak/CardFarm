using System;
using CameraMove.Core;
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

        private SafeAreaDragObserver _dragObserver;
        private Transform _cameraTransform;

        [Inject]
        private void Constructor(SafeAreaDragObserver dragObserver, CameraProvider cameraProvider)
        {
            _dragObserver = dragObserver;
            _cameraTransform = cameraProvider.Value.transform;
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

            _dragSubscription = _dragObserver.SmoothedDelta.Subscribe(MoveCamera);
        }

        private void StopObservingDrag()
        {
            _dragSubscription?.Dispose();
        }

        private void MoveCamera(Vector2 dragDelta)
        {
            Vector3 moveDirection = new Vector3(-dragDelta.x, 0f, -dragDelta.y);

            _cameraTransform.position += moveDirection * _speed * Time.deltaTime;
        }
    }
}
