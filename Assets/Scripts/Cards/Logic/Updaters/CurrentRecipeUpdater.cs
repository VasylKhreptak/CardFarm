using System;
using Cards.Data;
using ScriptableObjects.Scripts.Cards.Recipes;
using UniRx;
using UnityEngine;

namespace Cards.Logic.Updaters
{
    public class CurrentRecipeUpdater : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;
        [SerializeField] private CardRecipes _cardRecipes;

        private IDisposable _isTopCardSubscription;
        private IDisposable _isCardSingleSubscription;
        private IDisposable _cardStatsSubscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            _cardData ??= GetComponentInParent<CardData>();
        }

        private void OnEnable()
        {
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

                    bool hasRecipe = _cardRecipes.TryFindRecipe(_cardData.GroupCards, out CardRecipe recipe);

                    if (hasRecipe == false)
                    {
                        ResetCurrentRecipe();
                        return;
                    }

                    _cardData.CurrentRecipe.Value = recipe;
                });
        }

        private void ResetCurrentRecipe()
        {
            _cardData.CurrentRecipe.Value = null;
        }
    }
}
