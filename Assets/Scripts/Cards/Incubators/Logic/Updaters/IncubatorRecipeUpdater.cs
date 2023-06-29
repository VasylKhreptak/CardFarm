using System;
using System.Collections.Generic;
using System.Linq;
using Cards.Core;
using Cards.Incubators.Data;
using ScriptableObjects.Scripts.Cards.Incubators.Recipes;
using UniRx;
using UnityEngine;

namespace Cards.Incubators.Logic.Updaters
{
    public class IncubatorRecipeUpdater : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private IncubatorData _cardData;
        [SerializeField] private IncubatorRecipes _recipes;

        private IDisposable _isTopCardSubscription;
        private IDisposable _isCardSingleSubscription;
        private IDisposable _cardStatsSubscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            _cardData ??= GetComponentInParent<IncubatorData>();
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

                    List<Card> bottomCards = _cardData.BottomCards.Select(x => x.Card.Value).ToList();

                    bool hasRecipe = _recipes.TryFindRecipe(bottomCards, out IncubatorRecipe recipe);

                    if (hasRecipe == false)
                    {
                        ResetCurrentRecipe();
                        return;
                    }

                    _cardData.CurrentIncubatorRecipe.Value = recipe;
                });
        }

        private void ResetCurrentRecipe()
        {
            _cardData.CurrentIncubatorRecipe.Value = null;
        }
    }
}
