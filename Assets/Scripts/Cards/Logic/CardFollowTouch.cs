using System;
using Cards.Data;
using Constraints.CardTable;
using Physics;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic
{
    public class CardFollowTouch : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        [Header("PReferences")]
        [SerializeField] private LayerMask _floorLayerMask;
        [SerializeField] private float _speed = 15f;

        private IDisposable _isSelectedSubscription;
        private IDisposable _followTouchSubscription;

        private ScreenRaycaster _screenRaycaster;
        private CardsTableBounds _cardsTableBounds;

        [Inject]
        private void Constructor(ScreenRaycaster screenRaycaster, CardsTableBounds cardsTableBounds)
        {
            _screenRaycaster = screenRaycaster;
            _cardsTableBounds = cardsTableBounds;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<CardData>(true);
        }

        private void OnEnable()
        {
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
        }

        #endregion

        private void StartObserving()
        {
            StopObserving();

            _isSelectedSubscription = _cardData.IsSelected.Subscribe(isSelected =>
            {
                if (isSelected)
                {
                    StartFollowingTouch();
                }
                else
                {
                    StopFollowingTouch();
                }
            });
        }

        private void StopObserving()
        {
            StopFollowingTouch();
            _isSelectedSubscription?.Dispose();
        }

        private void StartFollowingTouch()
        {
            StopFollowingTouch();

            Vector3 mousePosition = Input.mousePosition;
            if (_screenRaycaster.Raycast(mousePosition, _floorLayerMask, out RaycastHit hit))
            {
                Vector3 difference = _cardData.transform.position - hit.point;
                Vector2 offset = new Vector2(difference.x, difference.z);
                _followTouchSubscription = Observable.EveryUpdate().Subscribe(_ => FollowTouchStep(offset));
            }
        }

        private void StopFollowingTouch()
        {
            _followTouchSubscription?.Dispose();
        }

        private void FollowTouchStep(Vector2 horOffset)
        {
            if (Input.touchCount > 1)
            {
                StopFollowingTouch();
                return;
            }

            Vector3 mousePosition = Input.mousePosition;

            if (_screenRaycaster.Raycast(mousePosition, _floorLayerMask, out RaycastHit hit))
            {
                float startHeight = _cardData.transform.position.y;
                Vector3 cardPosition = _cardData.transform.position;
                Vector3 targetCardPosition = hit.point + new Vector3(horOffset.x, 0f, horOffset.y);
                cardPosition = Vector3.Lerp(cardPosition, targetCardPosition, _speed * Time.deltaTime);
                cardPosition.y = _cardData.Height.Value;

                Vector3 clampedPosition = _cardsTableBounds.Clamp(_cardData.RectTransform, cardPosition);
                clampedPosition.y = startHeight;
                _cardData.transform.position = clampedPosition;
            }
        }
    }
}
