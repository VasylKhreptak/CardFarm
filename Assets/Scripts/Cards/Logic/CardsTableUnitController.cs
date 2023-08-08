using Cards.Data;
using UnityEngine;
using Zenject;

namespace Cards.Logic
{
    public class CardsTableUnitController : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        private CardsTable.Core.CardsTable _cardsTable;

        [Inject]
        private void Constructor(CardsTable.Core.CardsTable cardsTable)
        {
            _cardsTable = cardsTable;
        }

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
            Register();
        }

        private void OnDisable()
        {
            Unregister();
        }

        #endregion

        private void Register()
        {
            _cardsTable.AddCard(_cardData);
        }

        private void Unregister()
        {
            _cardsTable.RemoveCard(_cardData);
        }
    }
}
