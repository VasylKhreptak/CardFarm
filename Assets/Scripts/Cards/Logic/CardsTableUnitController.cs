using Cards.Data;
using Table.Core;
using UnityEngine;
using Zenject;
using IValidatable = EditorTools.Validators.Core.IValidatable;

namespace Cards.Logic
{
    public class CardsTableUnitController : MonoBehaviour, IValidatable
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

        public void OnValidate()
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
