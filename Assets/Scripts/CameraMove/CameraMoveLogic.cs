using System;
using CameraMove.Core;
using CameraZoom;
using UniRx;
using UnityEngine;
using Zenject;

namespace CameraMove
{
    public class CameraMoveLogic : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _transform;

        [Header("Preferences")]
        [SerializeField] private float _speed;
        [SerializeField] private Vector2 _min;
        [SerializeField] private Vector2 _max;

        private IDisposable _dragSubscription;

        private MapDragObserver _dragObserver;
        private CameraZoomLogic _cameraZoomLogic;

        [Inject]
        private void Constructor(MapDragObserver dragObserver,
            CameraZoomLogic cameraZoomLogic)
        {
            _dragObserver = dragObserver;
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
            Vector3 cameraPosition = _transform.position;

            cameraPosition += moveDirection * _speed * Time.deltaTime * _cameraZoomLogic.CameraDistance.Value;

            cameraPosition = new Vector3(
                Mathf.Clamp(cameraPosition.x, _min.x, _max.x),
                cameraPosition.y,
                Mathf.Clamp(cameraPosition.z, _min.y, _max.y));

            _transform.position = cameraPosition;
        }

        private void OnDrawGizmos()
        {
            float y = 0f;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(_min.x, y, _min.y), new Vector3(_min.x, y, _max.y));
            Gizmos.DrawLine(new Vector3(_min.x, y, _max.y), new Vector3(_max.x, y, _max.y));
            Gizmos.DrawLine(new Vector3(_max.x, y, _max.y), new Vector3(_max.x, y, _min.y));
            Gizmos.DrawLine(new Vector3(_max.x, y, _min.y), new Vector3(_min.x, y, _min.y));
        }
    }
}
