using System.Collections.Generic;
using System.Linq;
using Cards.Data;
using UniRx;
using UnityEngine;

namespace Cards
{
    public class CardsTable : MonoBehaviour
    {
        private IReactiveCollection<CardData> _cards = new ReactiveCollection<CardData>();

        public IReadOnlyReactiveCollection<CardData> CardsObservableCollection => _cards;

        public List<CardData> Cards => _cards.ToList();

        public void AddCard(CardData cardData)
        {
            _cards.Add(cardData);
        }

        public bool RemoveCard(CardData cardData)
        {
            return _cards.Remove(cardData);
        }

        public void ClearTable()
        {
            _cards.Clear();
        }
    }
}
