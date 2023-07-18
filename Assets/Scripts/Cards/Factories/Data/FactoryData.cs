using System.Collections.Generic;
using Cards.Data;
using ScriptableObjects.Scripts.Cards.AutomatedFactories.Recipes;
using UniRx;

namespace Cards.Factories.Data
{
    public class FactoryData : SellableCardData
    {
        public ReactiveProperty<FactoryRecipe> CurrentFactoryRecipe = new ReactiveProperty<FactoryRecipe>();

        public AutomatedFactoryCallbacks AutomatedFactoryCallbacks = new AutomatedFactoryCallbacks();

        public FactoryRecipes FactoryRecipes;

        public List<FactoryRecipe> PossibleFactoryRecipes = new List<FactoryRecipe>();
    }
}
