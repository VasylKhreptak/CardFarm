using System.Linq;
using Cards.Data;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Updaters
{
    public class FirstUpperCardUpdater : MonoBehaviour, IValidatable
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
            _cardData.FirstUpperCard.Value = null;
        }

        #endregion

        private void OnUpperCardsListUpdated()
        {
            if (_cardData.UpperCards.Count == 0)
            {
                _cardData.FirstUpperCard.Value = null;
                return;
            }

            _cardData.FirstUpperCard.Value = _cardData.UpperCards.First();
        }
    }
}
