using System;
using Cards.Data;
using EditorTools.Validators.Core;
using UniRx;
using UnityEngine;

namespace Cards.Logic.Events
{
    public class CardClickEventInvoker : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        [Header("Preferences")]
        [SerializeField] private float _maxDistance = 0.1f;

        private IDisposable _isSelectedSubscription;

        private Vector2 _previousClickPosition;

        #region MonoBehaviour

        public void OnValidate()
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
            _isSelectedSubscription = _cardData.IsSelected.Skip(1).Subscribe(IsSelectedValueChanged);
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
