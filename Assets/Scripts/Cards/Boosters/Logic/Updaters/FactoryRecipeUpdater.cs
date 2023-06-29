using System;
using System.Collections.Generic;
using System.Linq;
using Cards.AutomatedFactories.Data;
using Cards.Core;
using ScriptableObjects.Scripts.Cards.AutomatedFactories.Recipes;
using UniRx;
using UnityEngine;

namespace Cards.Boosters.Logic.Updaters
{
    public class FactoryRecipeUpdater : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private AutomatedCardFactoryData _cardData;
        [SerializeField] private FactoryRecipes _cardRecipes;

        private IDisposable _isTopCardSubscription;
        private IDisposable _isCardSingleSubscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            _cardData ??= GetComponentInParent<AutomatedCardFactoryData>();
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

            List<Card> bottomCards = _cardData.BottomCards.Select(x => x.Card.Value).ToList();

            bool hasRecipe = _cardRecipes.TryFindRecipe(bottomCards, out FactoryRecipe recipe);

            if (hasRecipe == false)
            {
                ResetCurrentRecipe();
                return;
            }

            _cardData.CurrentFactoryRecipe.Value = recipe;
        }

        private void ResetCurrentRecipe()
        {
            _cardData.CurrentFactoryRecipe.Value = null;
        }
    }
}
