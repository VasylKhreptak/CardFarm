using System.Collections.Generic;
using Cards.Data;
using CardsTable;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Updaters
{
    public class IsAnyGroupCardSelectedUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardDataHolder _cardData;

        private CompositeDisposable _subscriptions = new CompositeDisposable();

        private CurrentSelectedCardHolder _currentSelectedCardHolder;

        [Inject]
        private void Constructor(CurrentSelectedCardHolder currentSelectedCardHolder)
        {
            _currentSelectedCardHolder = currentSelectedCardHolder;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<CardDataHolder>(true);
        }

        private void OnEnable()
        {
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
            _cardData.IsAnyGroupCardSelected.Value = false;
        }

        #endregion

        private void StartObserving()
        {
            StopObserving();

            _currentSelectedCardHolder.SelectedCard.Subscribe(_ => OnCardEnvironmentChanged()).AddTo(_subscriptions);
            _cardData.Callbacks.onGroupCardsListUpdated += OnCardEnvironmentChanged;
        }

        private void StopObserving()
        {
            _subscriptions.Clear();
            _cardData.Callbacks.onGroupCardsListUpdated -= OnCardEnvironmentChanged;
        }

        private void OnCardEnvironmentChanged()
        {
            CardDataHolder selectedCard = _currentSelectedCardHolder.SelectedCard.Value;

            bool isAnyGroupCardSelected = false;

            if (selectedCard != null)
            {
                List<CardDataHolder> groupCards = _cardData.GroupCards;

                foreach (var groupCard in groupCards)
                {
                    if (groupCard.IsSelected.Value)
                    {
                        isAnyGroupCardSelected = true;
                        break;
                    }
                }
            }

            _cardData.IsAnyGroupCardSelected.Value = isAnyGroupCardSelected;
        }
    }
}
