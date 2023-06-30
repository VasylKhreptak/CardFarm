using Cards.Data;
using Constraints.CardTable;
using UnityEngine;
using Zenject;
using IValidatable = EditorTools.Validators.Core.IValidatable;

namespace Cards.Logic.Updaters
{
    public class IsInsideCardsTableUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        private CardsTableBounds _cardsTableBounds;

        [Inject]
        private void Constructor(CardsTableBounds cardsTableBounds)
        {
            _cardsTableBounds = cardsTableBounds;
        }

        #region MonoBehaivour

        public void OnValidate()
        {
            _cardData = GetComponentInParent<CardData>(true);
        }

        private void Update()
        {
            UpdateValue();
        }

        #endregion

        private void UpdateValue()
        {
            _cardData.IsInsideCardsTable.Value = _cardsTableBounds.IsInside(_cardData.Collider.bounds);
        }
    }
}
