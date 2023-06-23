using System;
using Animations.Shake.Position;
using Cards.Boosters.Data;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Cards.Boosters.Logic.Core
{
    public abstract class BoosterBaseLogic : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] protected BoosterCardData _cardData;

        [Header("Spawn Preferences")]
        [SerializeField] private float _minRange = 5f;
        [SerializeField] private float _maxRange = 7f;

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

        protected Vector3 GetRandomPosition()
        {
            float range = GetRange();

            Vector2 insideUnitCircle = Random.insideUnitCircle.normalized * range;

            Vector3 randomSphere = new Vector3(insideUnitCircle.x, 0f, insideUnitCircle.y);
            return _cardData.transform.position + randomSphere;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(_cardData.transform.position, _minRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_cardData.transform.position, _maxRange);
        }

        private float GetRange()
        {
            return Random.Range(_minRange, _maxRange);
        }
    }
}
