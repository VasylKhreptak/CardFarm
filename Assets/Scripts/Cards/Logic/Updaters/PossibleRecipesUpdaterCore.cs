using System;
using Cards.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Updaters
{
    public abstract class PossibleRecipesUpdaterCore<T> : MonoBehaviour, IValidatable where T : CardData
    {
        [Header("References")]
        [SerializeField] protected T _cardData;

        private IDisposable _isTopCardSubscription;
        private IDisposable _cardStatsSubscription;
        private IDisposable _isSingleCardSubscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<T>(true);
        }

        private void OnEnable()
        {
            UpdatePossibleRecipes();
            _cardData.Callbacks.onBecameHeadOfGroup += UpdatePossibleRecipes;
            _isTopCardSubscription = _cardData.IsTopCard.Skip(1).Subscribe(_ => OnCardStateUpdated());
            _isSingleCardSubscription = _cardData.IsSingleCard.Skip(1).Subscribe(_ => OnCardStateUpdated());
        }

        private void OnDisable()
        {
            _cardData.Callbacks.onBecameHeadOfGroup -= UpdatePossibleRecipes;
            _isTopCardSubscription?.Dispose();
            _cardStatsSubscription?.Dispose();
            _isSingleCardSubscription?.Dispose();
            ResetPossibleRecipes();
        }

        #endregion

        private void OnCardStateUpdated()
        {
            if (_cardData.IsSingleCard.Value)
            {
                UpdatePossibleRecipes();
            }
            else if (_cardData.IsTopCard.Value == false)
            {
                ResetPossibleRecipes();
            }
        }

        protected abstract void ResetPossibleRecipes();

        protected abstract void UpdatePossibleRecipes();
    }
}
