using System.Linq;
using Cards.Data;
using EditorTools.Validators.Core;
using UnityEngine;

namespace Cards.Logic.Updaters
{
    public class LowestCardUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        #region MonoBehaviour

        public void OnValidate()
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
