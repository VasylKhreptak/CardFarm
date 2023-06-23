using System.Linq;
using Cards.Data;
using UnityEngine;

namespace Cards.Logic.Updaters
{
    public class LowestCardUpdater : MonoBehaviour
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
