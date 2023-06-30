using System;
using Animations.Shake.Position;
using Cards.Boosters.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Boosters.Logic.Core
{
    public abstract class BoosterBaseLogic : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] protected BoosterCardData _cardData;

        private IDisposable _totalCardsSubscription;

        private CameraShakeAnimation _cameraShakeAnimation;

        [Inject]
        private void Constructor(CameraShakeAnimation cameraShakeAnimation)
        {
            _cameraShakeAnimation = cameraShakeAnimation;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<BoosterCardData>(true);
        }

        private void OnEnable()
        {
            StartObservingTotalCards();
            _cardData.Callbacks.onClicked += OnCLick;
        }

        private void OnDisable()
        {
            StopObservingTotalCards();
            _cardData.Callbacks.onClicked -= OnCLick;
        }

        #endregion

        private void StartObservingTotalCards()
        {
            StopObservingTotalCards();

            _totalCardsSubscription = _cardData.TotalCards.Take(1).Subscribe(OnTotalCardsChanged);
        }

        private void StopObservingTotalCards()
        {
            _totalCardsSubscription?.Dispose();
        }

        private void OnTotalCardsChanged(int totalCards)
        {
            _cardData.LeftCards.Value = totalCards;
            _cardData.BoosterCallabcks.OnRefilled?.Invoke(totalCards);
        }

        private void OnCLick()
        {
            if (_cardData.LeftCards.Value > 0)
            {
                _cameraShakeAnimation.Play();
                SpawnResultedCard();

                _cardData.LeftCards.Value--;
            }

            if (_cardData.LeftCards.Value == 0)
            {
                _cardData.gameObject.SetActive(false);
            }
        }

        protected abstract void SpawnResultedCard();
    }
}
