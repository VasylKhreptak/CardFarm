using Cards.Data;
using UniRx;

namespace CardsTable.Core
{
    public abstract class ManualCardSelector : CardTableObserver
    {
        private ReactiveCollection<CardData> _selectedCards = new ReactiveCollection<CardData>();

        public IReadOnlyReactiveCollection<CardData> SelectedCards => _selectedCards;

        protected override void OnAddedCard(CardData cardData)
        {
            if (IsCardAppropriate(cardData))
            {
                _selectedCards.Add(cardData);
            }
        }

        protected override void OnRemovedCard(CardData cardData)
        {
            if (IsCardAppropriate(cardData))
            {
                _selectedCards.Remove(cardData);
            }
        }

        protected override void ClearCards()
        {
            _selectedCards.Clear();
        }

        protected abstract bool IsCardAppropriate(CardData cardData);
    }
}
