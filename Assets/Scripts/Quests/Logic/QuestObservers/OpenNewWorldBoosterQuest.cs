using Cards.Boosters.Data;
using Cards.Core;
using Quests.Logic.QuestObservers.Core;
using Table.Core;
using UniRx;
using Zenject;

namespace Quests.Logic.QuestObservers
{
    public class OpenNewWorldBoosterQuest : QuestObserver
    {
        private CompositeDisposable _compositeDisposable = new CompositeDisposable();

        private CardsTableSelector _cardsTableSelector;

        [Inject]
        private void Constructor(CardsTableSelector cardsTableSelector)
        {
            _cardsTableSelector = cardsTableSelector;
        }

        public override void StartObserving()
        {
            if (_cardsTableSelector.SelectedCardsMap.TryGetValue(Card.NewWorldBooster, out var boosterCards))
            {
                if (boosterCards.Count > 0)
                {
                    BoosterCardData boosterCardData = boosterCards[0] as BoosterCardData;

                    StartObservingBoosterOpening(boosterCardData);

                    return;
                }
            }

            _cardsTableSelector.SelectedCardsMap.ObserveAdd().Subscribe(x =>
            {
                if (x.Key == Card.NewWorldBooster)
                {
                    x.Value.ObserveAdd().Subscribe(y =>
                    {
                        BoosterCardData boosterCardData = y.Value as BoosterCardData;
                        StopObserving();
                        StartObservingBoosterOpening(boosterCardData);
                    }).AddTo(_compositeDisposable);
                }
            }).AddTo(_compositeDisposable);
        }

        private void StartObservingBoosterOpening(BoosterCardData booster)
        {
            if (booster == null) return;

            booster.LeftCards.Where(x => x == 0).Subscribe(_ =>
            {
                _questData.IsCompleted.Value = true;
            }).AddTo(_compositeDisposable);
        }

        public override void StopObserving()
        {
            _compositeDisposable.Clear();
        }
    }
}
