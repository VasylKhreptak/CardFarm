using Cards.Data;
using Table.Core;
using UnityEngine;
using Zenject;

namespace Cards.Logic
{
    public class CardsTableUnitController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        private CardsTable _cardsTable;

        [Inject]
        private void Constructor(CardsTable cardsTable)
        {
            _cardsTable = cardsTable;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _cardData ??= GetComponentInParent<CardData>();
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
