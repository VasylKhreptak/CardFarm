using System;
using Cards.Boosters.Data;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Cards.Boosters.Logic.Core
{
    public abstract class BoosterBaseLogic : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] protected BoosterCardData _cardData;

        [Header("Spawn Preferences")]
        [SerializeField] private float _spawnRange = 5f;

        private IDisposable _totalCardsSubscription;

        #region MonoBehaviour

        private void OnEnable()
        {
            StartObservingTotalCards();
            _cardData.BoosterCallabcks.OnClick += OnCLick;
        }

        private void OnDisable()
        {
            StopObservingTotalCards();
            _cardData.BoosterCallabcks.OnClick -= OnCLick;
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
                SpawnCard();
                _cardData.LeftCards.Value--;
            }
            else
            {
                _cardData.gameObject.SetActive(false);
            }
        }

        protected abstract void SpawnCard();

        protected Vector3 GetRandomPosition()
        {
            Vector2 randomCircle = Random.insideUnitCircle * _spawnRange;
            Vector3 randomSphere = new Vector3(randomCircle.x, 0f, randomCircle.y);

            return _cardData.transform.position + randomSphere;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_cardData.transform.position, _spawnRange);
        }
    }
}
