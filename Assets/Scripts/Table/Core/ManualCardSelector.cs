using Cards.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Table.Core
{
    public abstract class ManualCardSelector : MonoBehaviour
    {
        private ReactiveCollection<CardData> _selectedCards = new ReactiveCollection<CardData>();

        public IReadOnlyReactiveCollection<CardData> SelectedCards => _selectedCards;
        
        private CompositeDisposable _subscriptions = new CompositeDisposable();

        private CardsTable _cardsTable;

        [Inject]
        private void Constructor(CardsTable cardsTable)
        {
            _cardsTable = cardsTable;
        }

        #region MonoBehaivour

        private void OnEnable()
        {
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
        }

        #endregion

        private void StartObserving()
        {
            foreach (var cardInTable in _cardsTable.Cards)
            {
                OnAddedCard(cardInTable);
            }

            var cards = _cardsTable.Cards;
            cards.ObserveAdd().Subscribe(x => OnAddedCard(x.Value)).AddTo(_subscriptions);
            cards.ObserveRemove().Subscribe(x => OnRemovedCard(x.Value)).AddTo(_subscriptions);
            cards.ObserveReset().Subscribe(_ => ClearCards()).AddTo(_subscriptions);
        }

        private void StopObserving()
        {
            _subscriptions.Clear();
            ClearCards();
        }

        private void OnAddedCard(CardData cardData)
        {
            if (IsCardAppropriate(cardData))
            {
                _selectedCards.Add(cardData);
            }
        }

        private void OnRemovedCard(CardData cardData)
        {
            if (IsCardAppropriate(cardData))
            {
                _selectedCards.Remove(cardData);
            }
        }

        private void ClearCards()
        {
            _selectedCards.Clear();
        }

        protected abstract bool IsCardAppropriate(CardData cardData);
    }
}
