using System.Collections.Generic;
using System.Linq;
using Cards.AutomatedFactories.Data;
using Cards.Core;
using Cards.Logic.Updaters;
using ScriptableObjects.Scripts.Cards.AutomatedFactories.Recipes;
using UnityEngine;

namespace Cards.Boosters.Logic.Updaters
{
    public class FactoryRecipeUpdater : RecipeUpdaterCore<AutomatedFactoryData>
    {
        [Header("References")]
        [SerializeField] private FactoryRecipes _cardRecipes;

        protected override void UpdateRecipe()
        {
            List<Card> bottomCards = _cardData.BottomCards.Select(x => x.Card.Value).ToList();

            _cardRecipes.TryFindRecipe(bottomCards, out FactoryRecipe recipe);

            _cardData.CurrentFactoryRecipe.Value = recipe;
        }

        protected override void ResetCurrentRecipe()
        {
            _cardData.CurrentFactoryRecipe.Value = null;
        }
    }
}
