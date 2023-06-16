using System;
using Cards.Data;
using CardsTable;
using ScriptableObjects.Scripts.Cards;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Updaters
{
    public class CanBeUnderSelectedCardUpdater : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;
        [SerializeField] private CompatibleCards _compatibleCards;

        private IDisposable _subscription;
        private IDisposable _cardGroupsIDsSubscription;

        private CurrentSelectedCardHolder _currentSelectedCardHolder;

        [Inject]
        private void Constructor(CurrentSelectedCardHolder currentSelectedCardHolder)
        {
            _currentSelectedCardHolder = currentSelectedCardHolder;
        }

        #region MonoBehaviour

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
            StopObserving();

            _subscription = Observable
                .CombineLatest(
                    _cardData.IsLowestGroupCard,
                    _currentSelectedCardHolder.SelectedCard,
                    (islLowestGroupCard, selectedCard) => (islLowestGroupCard, selectedCard))
                .Subscribe(tuple => OnConditionUpdated(tuple.islLowestGroupCard, tuple.selectedCard));
        }

        private void StopObserving()
        {
            _subscription?.Dispose();
            StopObservingCardGroupsID();
        }

        private void OnConditionUpdated(bool islLowestGroupCard, CardData selectedCard)
        {
            if (selectedCard == null)
            {
                _cardData.CanBeUnderSelectedCard.Value = false;
                StopObservingCardGroupsID();
                return;
            }

            StartObservingCardGroupsID(islLowestGroupCard, selectedCard);
        }

        private void StartObservingCardGroupsID(bool islLowestGroupCard, CardData selectedCard)
        {
            StopObservingCardGroupsID();

            _cardGroupsIDsSubscription = Observable
                .CombineLatest(selectedCard.GroupID, _cardData.GroupID)
                .Subscribe(list =>
                {
                    Debug.Log(selectedCard == _cardData);
                    
                    _cardData.CanBeUnderSelectedCard.Value = islLowestGroupCard
                        && selectedCard != _cardData
                        && list[0] != list[1]
                        && _compatibleCards.IsCompatible(selectedCard.Card.Value, _cardData.Card.Value);
                    
                    // Debug.Log(selectedCard.Card + " and " + _cardData.Card + " are compatible: " + _compatibleCards.IsCompatible(selectedCard.Card.Value, _cardData.Card.Value));
                });
        }

        private void StopObservingCardGroupsID()
        {
            _cardGroupsIDsSubscription?.Dispose();
        }
    }
}
