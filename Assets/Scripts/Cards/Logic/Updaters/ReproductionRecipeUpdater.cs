using System.Collections.Generic;
using System.Linq;
using Cards.Core;
using Cards.Data;
using ScriptableObjects.Scripts.Cards.ReproductionRecipes;
using UnityEngine;

namespace Cards.Logic.Updaters
{
    public class ReproductionRecipeUpdater : RecipeUpdaterCore<CardData>
    {
        [Header("References")]
        [SerializeField] private CardReproductionRecipes _cardRecipes;

        protected override void ResetCurrentRecipe()
        {
            _cardData.CurrentReproductionRecipe.Value = null;
        }

        protected override void UpdateRecipe()
        {
            List<Card> resources = _cardData.GroupCards.Select(x => x.Card.Value).ToList();

            _cardRecipes.TryGetRecipe(resources, out var recipe);

            _cardData.CurrentReproductionRecipe.Value = recipe;
        }
    }
}
