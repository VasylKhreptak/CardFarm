using System.Linq;
using Cards.Data;
using UnityEngine;

namespace Cards.Logic.Updaters
{
    public class TopCardUpdater : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        #region MonoBehaviour

        private void OnValidate()
        {
            _cardData ??= GetComponentInParent<CardData>();
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
