using Cards.Data;
using Table.Core;
using UniRx;
using Zenject;

namespace Quests.Logic.QuestObservers.Core
{
    public abstract class AllCardsQuestObserver : QuestObserver
    {
        private CompositeDisposable _subscriptions = new CompositeDisposable();

        private CardsTable _cardsTable;

        [Inject]
        private void Constructor(CardsTable cardsTable)
        {
            _cardsTable = cardsTable;
        }

        public override void StartObserving()
        {
            foreach (var tableCard in _cardsTable.ObservableCards)
            {
                OnCardAdded(tableCard);
            }

            _cardsTable.ObservableCards.ObserveAdd().Subscribe(x => OnCardAdded(x.Value)).AddTo(_subscriptions);
            _cardsTable.ObservableCards.ObserveRemove().Subscribe(x => OnCardRemoved(x.Value)).AddTo(_subscriptions);
            _cardsTable.ObservableCards.ObserveReset().Subscribe(_ => OnCardsCleared()).AddTo(_subscriptions);
        }

        public override void StopObserving()
        {
            _subscriptions.Clear();
        }

        protected abstract void OnCardAdded(CardData cardData);

        protected abstract void OnCardRemoved(CardData cardData);

        protected abstract void OnCardsCleared();
    }
}
