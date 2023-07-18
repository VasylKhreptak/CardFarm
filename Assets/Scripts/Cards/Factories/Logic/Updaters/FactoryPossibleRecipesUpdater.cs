using System.Collections.Generic;
using Cards.Factories.Data;
using Cards.Logic.Updaters;
using ScriptableObjects.Scripts.Cards.AutomatedFactories.Recipes;

namespace Cards.Factories.Logic.Updaters
{
    public class FactoryPossibleRecipesUpdater : PossibleRecipesUpdaterCore<FactoryData>
    {
        protected override void ResetPossibleRecipes()
        {
            _cardData.PossibleFactoryRecipes.Clear();
        }

        protected override void UpdatePossibleRecipes()
        {
            ResetPossibleRecipes();

            if (_cardData.FactoryRecipes.TryGetPossibleRecipes(_cardData.BottomCards, out List<FactoryRecipe> recipes))
            {
                _cardData.PossibleFactoryRecipes.AddRange(recipes);
            }
        }
    }
}
