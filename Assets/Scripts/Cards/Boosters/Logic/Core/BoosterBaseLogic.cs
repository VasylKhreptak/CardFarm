using System;
using Animations.Shake.Position;
using Cards.Boosters.Data;
using Constraints.CardTable;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Boosters.Logic.Core
{
    public abstract class BoosterBaseLogic : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] protected BoosterCardData _cardData;

        [Header("Spawn Preferences")]
        [SerializeField] protected float _minRange = 5f;
        [SerializeField] protected float _maxRange = 7f;

        private IDisposable _totalCardsSubscription;

        private CameraShakeAnimation _cameraShakeAnimation;
        private CardsTableBounds _cardsTableBounds;

        [Inject]
        private void Constructor(CameraShakeAnimation cameraShakeAnimation, CardsTableBounds cardsTableBounds)
        {
            _cameraShakeAnimation = cameraShakeAnimation;
            _cardsTableBounds = cardsTableBounds;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _cardData ??= GetComponentInParent<BoosterCardData>();
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

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(_cardData.transform.position, _minRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_cardData.transform.position, _maxRange);
        }
    }
}
