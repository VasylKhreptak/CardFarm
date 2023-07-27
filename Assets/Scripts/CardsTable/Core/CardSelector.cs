using System.Linq;
using Cards.Core;
using Cards.Data;
using UniRx;

namespace CardsTable.Core
{
    public class CardSelector : CardTableObserver
    {
        private ReactiveDictionary<Card, ReactiveCollection<CardData>> _selectedCardsMap = new ReactiveDictionary<Card, ReactiveCollection<CardData>>();

        public IReadOnlyReactiveDictionary<Card, ReactiveCollection<CardData>> SelectedCardsMap => _selectedCardsMap;

        protected override void OnAddedCard(CardData cardData)
        {
            if (_selectedCardsMap.ContainsKey(cardData.Card.Value) == false)
            {
                _selectedCardsMap.Add(cardData.Card.Value, new ReactiveCollection<CardData>());
            }

            _selectedCardsMap[cardData.Card.Value].Add(cardData);
        }

        protected override void OnRemovedCard(CardData cardData)
        {
            if (_selectedCardsMap.TryGetValue(cardData.Card.Value, out var collection))
            {
                collection.Remove(cardData);

                if (collection.Count == 0)
                {
                    _selectedCardsMap.Remove(cardData.Card.Value);
                }
            }
        }

        protected override void ClearCards()
        {
            _selectedCardsMap.Clear();
        }

        public int GetCount(Card card)
        {
            if (_selectedCardsMap.TryGetValue(card, out var cards))
            {
                return cards.Count;
            }

            return 0;
        }

        public int GetCount()
        {
            return _selectedCardsMap.Sum(x => x.Value.Count);
        }
    }
}
