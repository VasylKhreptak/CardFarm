using System;
using Cards.Data;
using UniRx;
using UnityEngine;

namespace Cards.Logic.Events
{
    public class CardClickEventInvoker : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        [Header("Preferences")]
        [SerializeField] private float _maxDistance = 0.1f;

        private IDisposable _isSelectedSubscription;

        private Vector2 _previousClickPosition;

        #region MonoBehaviour

        private void OnValidate()
        {
            _cardData ??= GetComponentInParent<CardData>();
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
            _isSelectedSubscription = _cardData.IsSelectedCard.Subscribe(IsSelectedValueChanged);
        }

        private void StopObserving()
        {
            _isSelectedSubscription?.Dispose();
        }

        private void IsSelectedValueChanged(bool isSelected)
        {
            if (isSelected)
            {
                OnMouseDown();
            }
            else
            {
                OnMouseUp();
            }
        }

        private void OnMouseDown()
        {
            _previousClickPosition = Input.mousePosition;
        }

        private void OnMouseUp()
        {
            Vector2 currentClickPosition = Input.mousePosition;

            if (Vector2.Distance(currentClickPosition, _previousClickPosition) < _maxDistance)
            {
                _cardData.Callbacks.onClicked?.Invoke();
            }
        }
    }
}
