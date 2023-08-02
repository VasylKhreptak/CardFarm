using Cards.Core;
using Cards.Data;
using CardsTable.Core;
using UniRx;
using UnityEngine;
using Zenject;

namespace Quests.Logic.Tutorials.Core
{
    public class QuestCardInteractionTutorial : QuestTextTutorial
    {
        [Header("Preferences")]
        [SerializeField] private Card _targetCard;

        private CompositeDisposable _cardTableSubscriptions = new CompositeDisposable();

        protected CardData _foundCard;

        private CardSelector _cardSelector;

        [Inject]
        private void Constructor(CardSelector cardSelector)
        {
            _cardSelector = cardSelector;
        }

        public override void StartTutorial()
        {
            base.StartTutorial();

            StopTutorial();
            StartObservingCardTable();
        }

        public override void StopTutorial()
        {
            base.StopTutorial();

            StopObservingCardTable();
            _foundCard = null;
        }

        private void StartObservingCardTable()
        {
            StopObservingCardTable();

            if (_cardSelector.SelectedCardsMap.TryGetValue(_targetCard, out var cards))
            {
                OnFoundCard(cards[0]);
                StopObservingCardTable();
                return;
            }

            _cardSelector.SelectedCardsMap.ObserveAdd().Subscribe(addEvent =>
            {
                if (addEvent.Key == _targetCard)
                {
                    OnFoundCard(addEvent.Value[0]);
                    StopObservingCardTable();
                }
            }).AddTo(_cardTableSubscriptions);
        }

        private void StopObservingCardTable()
        {
            _cardTableSubscriptions.Clear();
        }

        protected virtual void OnFoundCard(CardData cardData)
        {
            _foundCard = cardData;
        }
    }
}
