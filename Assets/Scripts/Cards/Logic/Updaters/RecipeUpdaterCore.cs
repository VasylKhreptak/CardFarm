using System;
using Cards.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Updaters
{
    public abstract class RecipeUpdaterCore<T> : MonoBehaviour, IValidatable where T : CardData
    {
        [Header("References")]
        [SerializeField] protected T _cardData;

        private IDisposable _isTopCardSubscription;
        private IDisposable _isCardSingleSubscription;
        private IDisposable _cardStatsSubscription;

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
            OnBecameHeadOfGroup();
            _cardData.Callbacks.onBecameHeadOfGroup += OnBecameHeadOfGroup;
            _isTopCardSubscription = _cardData.IsTopCard.Where(x => x == false).Subscribe(_ => ResetCurrentRecipe());
            _isCardSingleSubscription = _cardData.IsSingleCard.Where(x => x).Subscribe(_ => ResetCurrentRecipe());
        }

        private void OnDisable()
        {
            _cardData.Callbacks.onBecameHeadOfGroup -= OnBecameHeadOfGroup;
            _isTopCardSubscription?.Dispose();
            _isCardSingleSubscription?.Dispose();
            _cardStatsSubscription?.Dispose();
            ResetCurrentRecipe();
        }

        #endregion

        private void OnBecameHeadOfGroup()
        {
            _cardStatsSubscription?.Dispose();
            _cardStatsSubscription = Observable
                .CombineLatest(_cardData.IsTopCard, _cardData.IsSingleCard)
                .Subscribe(list =>
                {
                    if (list[0] == false || list[1])
                    {
                        ResetCurrentRecipe();
                        return;
                    }

                    UpdateRecipe();
                });
        }

        protected abstract void ResetCurrentRecipe();

        protected abstract void UpdateRecipe();
    }
}
