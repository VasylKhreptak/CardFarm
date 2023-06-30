using System.Linq;
using Cards.Data;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Updaters
{
    public class TopCardUpdater : MonoBehaviour, IValidatable
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
            _cardData.Callbacks.onUpperCardsListUpdated += OnUpperCardsListUpdated;
        }

        private void OnDisable()
        {
            _cardData.Callbacks.onUpperCardsListUpdated -= OnUpperCardsListUpdated;
        }

        #endregion

        private void OnUpperCardsListUpdated()
        {
            if (_cardData.UpperCards.Count == 0)
            {
                _cardData.TopCard.Value = null;
                return;
            }

            _cardData.TopCard.Value = _cardData.UpperCards.First();
        }
    }
}
