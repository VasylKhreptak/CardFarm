using Cards.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace CardsTable.Core
{
    public abstract class CardTableObserver : MonoBehaviour
    {
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
            SyncCards();

            var cards = _cardsTable.Cards;
            cards.ObserveAdd().Subscribe(x => OnAddedCard(x.Value)).AddTo(_subscriptions);
            cards.ObserveRemove().Subscribe(x => OnRemovedCard(x.Value)).AddTo(_subscriptions);
            cards.ObserveReset().Subscribe(_ => ClearCards()).AddTo(_subscriptions);
        }

        private void SyncCards()
        {
            ClearCards();
            foreach (var cardInTable in _cardsTable.Cards)
            {
                OnAddedCard(cardInTable);
            }
        }

        private void StopObserving()
        {
            _subscriptions.Clear();
            ClearCards();
        }

        protected abstract void OnAddedCard(CardData cardData);

        protected abstract void OnRemovedCard(CardData cardData);

        protected abstract void ClearCards();
    }
}
