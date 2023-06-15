using System.Collections.Generic;
using System.Linq;
using Cards.Data;
using UniRx;
using UnityEngine;

namespace Cards
{
    public class CardsTable : MonoBehaviour
    {
        private IReactiveCollection<CardData> _observableCards = new ReactiveCollection<CardData>();

        public IReadOnlyReactiveCollection<CardData> ObservableCards => _observableCards;

        public List<CardData> CardsList => _observableCards.ToList();

        public void AddCard(CardData cardData)
        {
            _observableCards.Add(cardData);
        }

        public bool RemoveCard(CardData cardData)
        {
            return _observableCards.Remove(cardData);
        }

        public void ClearTable()
        {
            _observableCards.Clear();
        }
    }
}
