using Cards.Data;
using ScriptableObjects.Scripts.Cards.Recipes;
using UnityEngine;

namespace Cards.Logic.Updaters
{
    public class CurrentRecipeUpdater : RecipeUpdater<CardData>
    {
        [Header("References")]
        [SerializeField] private CardRecipes _cardRecipes;

        protected override void ResetCurrentRecipe()
        {
            _cardData.CurrentRecipe.Value = null;
        }

        protected override void UpdateRecipe()
        {
            _cardRecipes.TryFindRecipe(_cardData.GroupCards, out CardRecipe recipe);

            _cardData.CurrentRecipe.Value = recipe;
        }
    }
}
