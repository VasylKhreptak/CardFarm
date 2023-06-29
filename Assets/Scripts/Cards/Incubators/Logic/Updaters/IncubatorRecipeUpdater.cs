using System.Collections.Generic;
using System.Linq;
using Cards.Core;
using Cards.Incubators.Data;
using Cards.Logic.Updaters;
using ScriptableObjects.Scripts.Cards.Incubators.Recipes;
using UnityEngine;

namespace Cards.Incubators.Logic.Updaters
{
    public class IncubatorRecipeUpdater : RecipeUpdater<IncubatorData>
    {
        [Header("References")]
        [SerializeField] private IncubatorRecipes _recipes;

        protected override void ResetCurrentRecipe()
        {
            _cardData.CurrentIncubatorRecipe.Value = null;
        }

        protected override void UpdateRecipe()
        {
            List<Card> bottomCards = _cardData.BottomCards.Select(x => x.Card.Value).ToList();

            _recipes.TryFindRecipe(bottomCards, out IncubatorRecipe recipe);

            _cardData.CurrentIncubatorRecipe.Value = recipe;
        }
    }
}
