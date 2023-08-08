using System;
using CameraManagement.CameraAim.Core;
using CameraManagement.CameraMove.Core;
using Cards.Data;
using CardsTable;
using Constraints;
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
        [SerializeField] private float _centeringDeadZone = 5f;
        [SerializeField] private LayerMask _floorLayerMask;

        private IDisposable _dragSubscription;
        private IDisposable _selectedCardSubscription;
        private IDisposable _cameraCenteringSubscription;

        private MapDragObserver _dragObserver;
        private CurrentSelectedCardHolder _currentSelectedCardHolder;
        private CameraAimer _cameraAimer;
        private CameraBounds _cameraBounds;

        [Inject]
        private void Constructor(MapDragObserver dragObserver,
            CurrentSelectedCardHolder currentSelectedCardHolder,
            CameraAimer cameraAimer,
            CameraBounds cameraBounds)
        {
            _dragObserver = dragObserver;
            _currentSelectedCardHolder = currentSelectedCardHolder;
            _cameraAimer = cameraAimer;
            _cameraBounds = cameraBounds;
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

        private void OnSelectedCardChanged(CardDataHolder cardData)
        {
            _cameraCenteringSubscription?.Dispose();

            if (cardData == null) return;

            _cameraCenteringSubscription = Observable.EveryUpdate().Subscribe(_ => MoveCameraToSelectedCard(cardData.transform.position));
        }

        private void MoveCameraToSelectedCard(Vector3 targetPosition)
        {
            if (_cameraAimer.IsAiming.Value) return;

            Vector3 cameraPosition = _transform.position;

            targetPosition.y = cameraPosition.y;
            Vector3 worldDirection = targetPosition - cameraPosition;
            Vector2 direction = new Vector2(worldDirection.x, worldDirection.z);

            float cameraDistance = 0;

            if (RaycastFloor(out var hit))
            {
                cameraDistance = hit.distance;
            }

            if (direction.magnitude > _centeringDeadZone * cameraDistance)
            {
                MoveCamera(direction * (_cardCenteringSpeed * Time.deltaTime));
            }
        }

        private void MoveCamera(Vector2 direction)
        {
            Vector3 moveDirection = new Vector3(direction.x, 0f, direction.y);
            Vector3 cameraPosition = _transform.position;

            cameraPosition += moveDirection * _speed;

            cameraPosition = ClampPosition(cameraPosition);

            _transform.position = cameraPosition;
        }

        private bool RaycastFloor(out RaycastHit hit)
        {
            Ray ray = new Ray(_transform.position, _transform.forward);
            return UnityEngine.Physics.Raycast(ray, out hit, float.MaxValue, _floorLayerMask);
        }

        private Vector3 ClampPosition(Vector3 position)
        {
            Ray ray = new Ray(position, _transform.forward);

            if (UnityEngine.Physics.Raycast(ray, out var hit, float.MaxValue, _floorLayerMask))
            {
                Vector3 hitPoint = hit.point;
                float distance = hit.distance;

                hitPoint = _cameraBounds.Clamp(hitPoint);

                hitPoint += -_transform.forward * distance;

                return hitPoint;
            }

            return position;
        }
    }
}
