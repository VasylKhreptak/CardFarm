using System;
using CameraManagement.CameraMove.Core;
using Cards.Data;
using Table;
using UniRx;
using UnityEngine;
using Zenject;

namespace CameraManagement.CameraMove
{
    public class CameraMover : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _transform;

        [Header("Preferences")]
        [SerializeField] private float _speed;
        [SerializeField] private float _cardCenteringSpeed = 1;

        private IDisposable _dragSubscription;
        private IDisposable _selectedCardSubscription;
        private IDisposable _cameraCenteringSubscription;

        private MapDragObserver _dragObserver;
        private CurrentSelectedCardHolder _currentSelectedCardHolder;

        [Inject]
        private void Constructor(MapDragObserver dragObserver,
            CurrentSelectedCardHolder currentSelectedCardHolder)
        {
            _dragObserver = dragObserver;
            _currentSelectedCardHolder = currentSelectedCardHolder;
        }

        #region MonoBehaviour

        private void OnEnable()
        {
            StartObservingDrag();
            StartObservingSelectedCard();
        }

        private void OnDisable()
        {
            StopObservingDrag();
            StopObservingSelectedCard();
            _cameraCenteringSubscription?.Dispose();
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

        private void StartObservingSelectedCard()
        {
            StopObservingSelectedCard();
            _selectedCardSubscription = _currentSelectedCardHolder.SelectedCard.Subscribe(OnSelectedCardChanged);
        }

        private void StopObservingSelectedCard()
        {
            _selectedCardSubscription?.Dispose();
        }

        private void OnSelectedCardChanged(CardData cardData)
        {
            _cameraCenteringSubscription?.Dispose();

            if (cardData == null) return;

            _cameraCenteringSubscription = Observable.EveryUpdate().Subscribe(_ => MoveCameraToSelectedCard(cardData.transform.position));
        }

        private void MoveCameraToSelectedCard(Vector3 targetPosition)
        {
            Vector3 cameraPosition = _transform.position;

            targetPosition.y = cameraPosition.y;
            Vector3 direction = targetPosition - cameraPosition;
            MoveCamera(new Vector2(direction.x, direction.z) * (_cardCenteringSpeed * Time.deltaTime));
        }

        private void MoveCamera(Vector2 direction)
        {
            Vector3 moveDirection = new Vector3(direction.x, 0f, direction.y);
            Vector3 cameraPosition = _transform.position;

            cameraPosition += moveDirection * _speed;

            _transform.position = cameraPosition;
        }
    }
}
