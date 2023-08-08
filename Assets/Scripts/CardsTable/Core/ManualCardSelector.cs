using Cards.Data;
using UniRx;

namespace CardsTable.Core
{
    public abstract class ManualCardSelector : CardTableObserver
    {
        private ReactiveCollection<CardDataHolder> _selectedCards = new ReactiveCollection<CardDataHolder>();

        public IReadOnlyReactiveCollection<CardDataHolder> SelectedCards => _selectedCards;

        protected override void OnAddedCard(CardDataHolder cardData)
        {
            if (IsCardAppropriate(cardData))
            {
                _selectedCards.Add(cardData);
            }
        }

        protected override void OnRemovedCard(CardDataHolder cardData)
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

        protected abstract bool IsCardAppropriate(CardDataHolder cardData);
    }
}
