using System;
using System.Collections.Generic;
using System.Linq;
using Cards.Core;
using Cards.Data;
using ScriptableObjects.Scripts.Cards.ReproductionRecipes;
using UniRx;
using UnityEngine;

namespace Cards.Logic.Updaters
{
    public class CurrentReproductionRecipeUpdater : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;
        [SerializeField] private CardReproductionRecipes _cardRecipes;

        private IDisposable _isTopCardSubscription;
        private IDisposable _isCardSingleSubscription;

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
            _isTopCardSubscription.Dispose();
            _isCardSingleSubscription.Dispose();
            ResetCurrentRecipe();
        }

        #endregion

        private void OnBecameHeadOfGroup()
        {
            if (_cardData.IsTopCard.Value == false || _cardData.IsSingleCard.Value)
            {
                ResetCurrentRecipe();
                return;
            }

            List<Card> resources = _cardData.GroupCards.Select(x => x.Card.Value).ToList();

            bool hasRecipe = _cardRecipes.TryGetRecipe(resources, out var recipe);

            if (hasRecipe == false)
            {
                ResetCurrentRecipe();
                return;
            }

            _cardData.CurrentReproductionRecipe.Value = recipe;
        }

        private void ResetCurrentRecipe()
        {
            _cardData.CurrentReproductionRecipe.Value = null;
        }
    }
}
