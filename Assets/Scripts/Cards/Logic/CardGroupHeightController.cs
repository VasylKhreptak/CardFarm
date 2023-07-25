using System;
using Cards.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic
{
    public class CardGroupHeightController : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        private IDisposable _upperCardHeightSubscription;
        private IDisposable _isUpdatingHeightSubscription;

        private IDisposable _upperCardSubscription;

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
            _isUpdatingHeightSubscription?.Dispose();
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
            _isUpdatingHeightSubscription?.Dispose();

            if (upperCardData == null)
            {
                position.y = _cardData.BaseHeight;
                _cardData.Height.Value = position.y;
            }
            else
            {
                _isUpdatingHeightSubscription = _cardData.CardSelectedHeightController
                    .IsUpdatingHeightProperty
                    .Subscribe(isUpdating =>
                    {
                        if (isUpdating == false)
                        {
                            _upperCardHeightSubscription?.Dispose();
                            _upperCardHeightSubscription = upperCardData.Height.Subscribe(height =>
                            {
                                position.y = height;
                                _cardData.Height.Value = position.y;
                            });
                        }
                        else
                        {
                            _upperCardHeightSubscription?.Dispose();
                        }
                    });
            }

            _cardData.transform.position = position;
        }
    }
}
