using System.Linq;
using Cards.Data;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Updaters
{
    public class LowestCardUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

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
            _cardData.Callbacks.onBottomCardsListUpdated += OnBottomCardsListUpdated;
        }

        private void OnDisable()
        {
            _cardData.Callbacks.onBottomCardsListUpdated -= OnBottomCardsListUpdated;
        }

        #endregion

        private void OnBottomCardsListUpdated()
        {
            if (_cardData.BottomCards.Count == 0)
            {
                _cardData.LowestCard.Value = null;
                return;
            }

            _cardData.LowestCard.Value = _cardData.BottomCards.Last();
        }
    }
}
