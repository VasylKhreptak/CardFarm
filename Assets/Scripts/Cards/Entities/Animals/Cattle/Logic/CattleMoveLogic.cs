using Cards.Entities.Animals.Cattle.Data;
using Constraints.CardTable;
using EditorTools.Validators.Core;
using Extensions;
using UnityEngine;
using Zenject;
using IValidatable = EditorTools.Validators.Core.IValidatable;

namespace Cards.Entities.Animals.Cattle.Logic
{
    public class CattleMoveLogic : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CattleCardData _cardData;

        [Header("Spawn Preferences")]
        [SerializeField] private float _minRange = 5f;
        [SerializeField] private float _maxRange = 7f;

        private CardsTableBounds _cardsTableBounds;

        [Inject]
        private void Constructor(CardsTableBounds cardsTableBounds)
        {
            _cardsTableBounds = cardsTableBounds;
        }

        #region MonoBehaviour

        public void OnValidate()
        {
            _cardData = GetComponentInParent<CattleCardData>(true);
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
            _cardData.CattleCallbacks.OnItemSpawnedNoArgs += OnCattleSpawnedCard;
        }

        private void StopObserving()
        {
            _cardData.CattleCallbacks.OnItemSpawnedNoArgs -= OnCattleSpawnedCard;
        }

        private void OnCattleSpawnedCard()
        {
            JumpInRandomDirection();
        }

        private void JumpInRandomDirection()
        {
            Vector3 position = _cardsTableBounds.GetRandomPositionInRange(_cardData.Collider.bounds, _minRange, _maxRange);

            _cardData.Animations.JumpAnimation.Play(position);

            if (_cardData.CanSortingLayerChange)
            {
                _cardData.RenderOnTop();
            }
        }
    }
}
