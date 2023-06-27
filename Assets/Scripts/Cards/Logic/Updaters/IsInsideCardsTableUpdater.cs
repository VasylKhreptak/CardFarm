using Cards.Data;
using Constraints.CardTable;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Updaters
{
    public class IsInsideCardsTableUpdater : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        private CardsTableConstraints _cardsTableConstraints;

        [Inject]
        private void Constructor(CardsTableConstraints cardsTableConstraints)
        {
            _cardsTableConstraints = cardsTableConstraints;
        }

        #region MonoBehaivour

        private void OnValidate()
        {
            _cardData ??= GetComponentInParent<CardData>();
        }

        private void Update()
        {
            UpdateValue();
        }

        #endregion

        private void UpdateValue()
        {
            _cardData.IsInsideCardsTable.Value = _cardsTableConstraints.IsInside(_cardData.Collider.bounds);
        }
    }
}
