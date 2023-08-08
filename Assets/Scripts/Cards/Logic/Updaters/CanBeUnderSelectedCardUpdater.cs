using System.Collections.Generic;
using System.Linq;
using Cards.Data;
using CardsTable;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Updaters
{
    public class CanBeUnderSelectedCardUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        private CompositeDisposable _subscriptions = new CompositeDisposable();

        private CardData _previousSelectedCard;

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
            _cardData = GetComponentInParent<CardData>(true);
        }

        private void OnEnable()
        {
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
            _cardData.CanBeUnderSelectedCard.Value = false;
        }

        #endregion

        private void StartObserving()
        {
            StopObserving();

            _cardData.IsLastGroupCard.Subscribe(_ => OnCardsEnvironmentChanged()).AddTo(_subscriptions);
            _cardData.IsCompatibleWithSelectedCard.Subscribe(_ => OnCardsEnvironmentChanged()).AddTo(_subscriptions);
            _cardData.IsSelected.Subscribe(_ => OnCardsEnvironmentChanged()).AddTo(_subscriptions);
            _currentSelectedCardHolder.SelectedCard.Subscribe(selectedCard =>
            {
                OnSelectedCardChanged(selectedCard);
                OnCardsEnvironmentChanged();
            }).AddTo(_subscriptions);
        }

        private void StopObserving()
        {
            _subscriptions.Clear();
            StopObservingSelectedCardAdjacentCards(_previousSelectedCard);
            _previousSelectedCard = null;
        }

        private void OnCardsEnvironmentChanged()
        {
            bool canBeUnderSelectedCard = false;

            bool isLowestGroupCard = _cardData.IsLastGroupCard.Value;
            bool isCompatibleWithSelectedCard = _cardData.IsCompatibleWithSelectedCard.Value;
            bool isSelected = _cardData.IsSelected.Value;
            bool isCardsSame = _cardData == _currentSelectedCardHolder.SelectedCard.Value;

            CardData selectedCard = _currentSelectedCardHolder.SelectedCard.Value;

            List<CardData> selectedCardBottomCards = new List<CardData>(0);
            List<CardData> selectedCardUpperCards = new List<CardData>(0);

            if (selectedCard != null)
            {
                selectedCardBottomCards = selectedCard.BottomCards;
                selectedCardUpperCards = selectedCard.UpperCards;
            }

            bool isCardInSelectedCardBottomCards = selectedCardBottomCards.Contains(_cardData);
            bool isCardLastInSelectedCardUpperCards = selectedCardUpperCards.LastOrDefault() == _cardData;

            canBeUnderSelectedCard =
                isLowestGroupCard == false ? isCardLastInSelectedCardUpperCards : isLowestGroupCard
                    && isSelected == false
                    && isCompatibleWithSelectedCard
                    && isCardsSame == false
                    && isCardInSelectedCardBottomCards == false;


            _cardData.CanBeUnderSelectedCard.Value = canBeUnderSelectedCard;
        }

        private void OnSelectedCardChanged(CardData selectedCard)
        {
            StopObservingSelectedCardAdjacentCards(_previousSelectedCard);

            StartObservingSelectedCardAdjacentCards(selectedCard);

            _previousSelectedCard = selectedCard;
        }

        private void StartObservingSelectedCardAdjacentCards(CardData cardData)
        {
            if (cardData == null) return;

            StopObservingSelectedCardAdjacentCards(_previousSelectedCard);
            StopObservingSelectedCardAdjacentCards(cardData);

            cardData.Callbacks.onBottomCardsListUpdated += OnCardsEnvironmentChanged;
            cardData.Callbacks.onUpperCardsListUpdated += OnCardsEnvironmentChanged;
        }

        private void StopObservingSelectedCardAdjacentCards(CardData cardData)
        {
            if (cardData == null) return;

            cardData.Callbacks.onBottomCardsListUpdated -= OnCardsEnvironmentChanged;
            cardData.Callbacks.onUpperCardsListUpdated -= OnCardsEnvironmentChanged;
        }
    }
}
