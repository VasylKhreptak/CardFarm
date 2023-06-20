using Cards.Core;
using Cards.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Table.Core
{
    public class CardsTableSelector : MonoBehaviour
    {
        [Header("Preferences")]
        [SerializeField] private Card _selectorType;

        private IReactiveCollection<CardData> _selectedCards = new ReactiveCollection<CardData>();

        public IReadOnlyReactiveCollection<CardData> SelectedCards => _selectedCards;

        private CompositeDisposable _subscriptions = new CompositeDisposable();

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
            _selectedCards.Clear();
        }

        private void OnCardAdded(CardData card)
        {
            if (card.Card.Value == _selectorType)
            {
                _selectedCards.Add(card);
            }
        }

        private void OnCardRemoved(CardData card)
        {
            if (card.Card.Value == _selectorType)
            {
                _selectedCards.Remove(card);
            }
        }

        private void OnCardsReset()
        {
            _selectedCards.Clear();
        }
    }
}
