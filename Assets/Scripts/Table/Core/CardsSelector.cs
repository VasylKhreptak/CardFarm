using Cards.Core;
using Cards.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Table.Core
{
    public class CardsSelector : MonoBehaviour
    {
        private ReactiveDictionary<Card, ReactiveCollection<CardData>> _selectedCardsMap = new ReactiveDictionary<Card, ReactiveCollection<CardData>>();

        private CompositeDisposable _subscriptions = new CompositeDisposable();

        public IReadOnlyReactiveDictionary<Card, ReactiveCollection<CardData>> SelectedCardsMap => _selectedCardsMap;

        private CardsTable _cardsTable;

        [Inject]
        private void Constructor(CardsTable cardsTable)
        {
            _cardsTable = cardsTable;
        }

        #region MonoBehaviour

        private void Awake()
        {
            StartObserving();
        }

        private void OnDestroy()
        {
            StopObserving();
        }

        #endregion

        private void StartObserving()
        {
            SyncSelectedCard();

            IReadOnlyReactiveCollection<CardData> reactiveCollection = _cardsTable.ObservableCards;

            reactiveCollection.ObserveAdd().Select(x => x.Value).Subscribe(OnCardAdded).AddTo(_subscriptions);
            reactiveCollection.ObserveRemove().Select(x => x.Value).Subscribe(OnCardRemoved).AddTo(_subscriptions);
            reactiveCollection.ObserveReset().Subscribe(_ => OnCardsReset()).AddTo(_subscriptions);
        }

        private void StopObserving()
        {
            ClearCards();
            _subscriptions.Clear();
        }

        private void SyncSelectedCard()
        {
            ClearCards();

            foreach (var cardInTable in _cardsTable.ObservableCards)
            {
                OnCardAdded(cardInTable);
            }
        }

        private void ClearCards()
        {
            _selectedCardsMap.Clear();
        }

        private void OnCardAdded(CardData card)
        {
            if (_selectedCardsMap.ContainsKey(card.Card.Value) == false)
            {
                _selectedCardsMap.Add(card.Card.Value, new ReactiveCollection<CardData>());
            }

            _selectedCardsMap[card.Card.Value].Add(card);
        }

        private void OnCardRemoved(CardData card)
        {
            if (_selectedCardsMap.TryGetValue(card.Card.Value, out var collection))
            {
                collection.Remove(card);

                if (collection.Count == 0)
                {
                    _selectedCardsMap.Remove(card.Card.Value);
                }
            }
        }

        private void OnCardsReset()
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
    }
}
