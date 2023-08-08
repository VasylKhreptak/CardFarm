using System;
using Cards.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic
{
    public class CardHeightInheritor : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        private IDisposable _upperCardHeightSubscription;
        private IDisposable _upperCardSubscription;
        private IDisposable _frameDelaySubscription;

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
            UpdateHeight(null);
            StartObservingUpperCard();
        }

        private void OnDisable()
        {
            StopObservingUpperCard();
            _frameDelaySubscription?.Dispose();
        }

        #endregion

        private void StartObservingUpperCard()
        {
            StopObservingUpperCard();
            _upperCardSubscription = _cardData.UpperCard.Subscribe(UpdateHeight);
        }

        private void StopObservingUpperCard()
        {
            _upperCardSubscription?.Dispose();
            _upperCardHeightSubscription?.Dispose();
        }

        private void UpdateHeight(CardData upperCardData)
        {
            Vector3 position = _cardData.transform.position;

            _upperCardHeightSubscription?.Dispose();

            if (upperCardData == null)
            {
                position.y = _cardData.BaseHeight;
                _cardData.Height.Value = position.y;
            }
            else
            {
                _frameDelaySubscription?.Dispose();
                _frameDelaySubscription = Observable.NextFrame().Subscribe(_ =>
                {
                    _upperCardHeightSubscription?.Dispose();
                    _upperCardHeightSubscription = upperCardData.Height.Subscribe(height =>
                    {
                        if (_cardData.CardSelectedHeightController.IsUpdatingHeightProperty.Value) return;

                        position.y = height;
                        _cardData.Height.Value = position.y;
                    });
                });
            }

            _cardData.transform.position = position;
        }
    }
}
