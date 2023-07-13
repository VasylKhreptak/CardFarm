using System;
using CameraMove.Core;
using Runtime.Screen;
using Table;
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
        [SerializeField] private float _moveByEdgeSpeed;
        [SerializeField] private Vector2 _min;
        [SerializeField] private Vector2 _max;

        private IDisposable _dragSubscription;

        private MapDragObserver _dragObserver;
        private ScreenEdges _screenEdges;
        private CurrentSelectedCardHolder _currentSelectedCardHolder;

        [Inject]
        private void Constructor(MapDragObserver dragObserver,
            ScreenEdges screenEdges,
            CurrentSelectedCardHolder currentSelectedCardHolder)
        {
            _dragObserver = dragObserver;
            _screenEdges = screenEdges;
            _currentSelectedCardHolder = currentSelectedCardHolder;
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

        private void Update()
        {
            TryMoveByEdge();
        }

        #endregion

        private void StartObservingDrag()
        {
            StopObservingDrag();

            _dragSubscription = _dragObserver.Delta.Subscribe(delta => MoveCamera(-delta));
        }

        private void StopObservingDrag()
        {
            _dragSubscription?.Dispose();
        }

        private void MoveCamera(Vector2 direction)
        {
            Vector3 moveDirection = new Vector3(direction.x, 0f, direction.y);
            Vector3 cameraPosition = _transform.position;

            cameraPosition += moveDirection * _speed;

            cameraPosition = new Vector3(
                Mathf.Clamp(cameraPosition.x, _min.x, _max.x),
                cameraPosition.y,
                Mathf.Clamp(cameraPosition.z, _min.y, _max.y));

            _transform.position = cameraPosition;
        }

        private void TryMoveByEdge()
        {
            if (_currentSelectedCardHolder.SelectedCard == null) return;

            MoveCamera(_screenEdges.DirectionFromCenter.Value * _moveByEdgeSpeed * Time.deltaTime);
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
