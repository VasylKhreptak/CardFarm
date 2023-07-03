using System;
using Cards.Entities.Animals.Cattle.Data;
using Constraints.CardTable;
using Extensions;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Entities.Animals.Cattle.Logic
{
    public class CattleJumpLogic : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CattleCardData _cardData;

        [Header("Preferences")]
        [SerializeField] private float _jumpInterval = 20f;

        [Header("Spawn Preferences")]
        [SerializeField] private float _minRange = 5f;
        [SerializeField] private float _maxRange = 7f;

        private IDisposable _jumpSubscription;

        private CardsTableBounds _cardsTableBounds;

        [Inject]
        private void Constructor(CardsTableBounds cardsTableBounds)
        {
            _cardsTableBounds = cardsTableBounds;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<CattleCardData>(true);
        }

        private void OnEnable()
        {
            StartJumping();
        }

        private void OnDisable()
        {
            StopJumping();
        }

        #endregion

        private void StartJumping()
        {
            StopJumping();
            _jumpSubscription = Observable.Interval(TimeSpan.FromSeconds(_jumpInterval)).Subscribe(_ => JumpInRandomDirection());
        }

        private void StopJumping()
        {
            _jumpSubscription?.Dispose();
        }

        private void JumpInRandomDirection()
        {
            Vector3 position = _cardsTableBounds.GetRandomPositionInRange(_cardData.Collider.bounds, _minRange, _maxRange);

            _cardData.Animations.JumpAnimation.Play(position);

            if (_cardData.CanSortingLayerChange)
            {
                _cardData.RenderOnTop();
            }

            _cardData.CattleCallbacks.onJumped?.Invoke();
        }
    }
}
