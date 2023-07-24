using System;
using CameraMove.Core;
using Runtime.Screen;
using Table;
using UniRx;
using UnityEngine;
using Zenject;

namespace CameraMove
{
    public class CameraMover : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _transform;

        [Header("Preferences")]
        [SerializeField] private float _speed;
        [SerializeField] private float _moveByEdgeSpeed;

        private IDisposable _dragSubscription;

        private MapDragObserver _dragObserver;
        private ScreenEdges _screenEdges;
        private CurrentSelectedCardHolder _currentSelectedCardHolder;

        public bool CanMove = true;

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

            _dragSubscription = _dragObserver.Delta.Subscribe(delta => TryMoveCamera(-delta));
        }

        private void StopObservingDrag()
        {
            _dragSubscription?.Dispose();
        }

        private void TryMoveCamera(Vector2 direction)
        {
            if (CanMove == false) return;

            Vector3 moveDirection = new Vector3(direction.x, 0f, direction.y);
            Vector3 cameraPosition = _transform.position;

            cameraPosition += moveDirection * _speed;

            _transform.position = cameraPosition;
        }

        private void TryMoveByEdge()
        {
            if (CanMove == false) return;

            if (_currentSelectedCardHolder.SelectedCard == null) return;

            TryMoveCamera(_screenEdges.DirectionFromCenter.Value * _moveByEdgeSpeed * Time.deltaTime);
        }
    }
}
