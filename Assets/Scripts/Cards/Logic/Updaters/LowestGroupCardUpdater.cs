using System.Linq;
using Cards.Data;
using UnityEngine;

namespace Cards.Logic.Updaters
{
    public class LowestGroupCardUpdater : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        #region MonoBehaviour

        private void OnEnable()
        {
            OnGroupCardsUpdated();
            StartObservingGroupCards();
        }

        private void OnDisable()
        {
            StopObservingGroupCards();
        }

        #endregion

        private void StartObservingGroupCards()
        {
            StopObservingGroupCards();

            _cardData.Callbacks.onGroupCardsListUpdated += OnGroupCardsUpdated;
        }

        private void StopObservingGroupCards()
        {
            _cardData.Callbacks.onGroupCardsListUpdated -= OnGroupCardsUpdated;
        }

        private void OnGroupCardsUpdated()
        {
            if(_cardData.GroupCards.Count == 0)
            {
                _cardData.LowestGroupCard.Value = null;
                return;
            }
            
            _cardData.LowestGroupCard.Value = _cardData.GroupCards.Last();
        }
    }
}
