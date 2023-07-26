using Cards.Data;
using Constraints.CardTable;
using UnityEngine;
using Zenject;

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

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
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
            _cardData.IsInsideCardsTable.Value = _cardsTableBounds.IsInside(_cardData.RectTransform);
        }
    }
}
