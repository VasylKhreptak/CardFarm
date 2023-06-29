using System;
using Cards.Core;
using ScriptableObjects.Scripts.Cards.AutomatedFactories.Recipes;

namespace Cards.AutomatedFactories.Data
{
    [Serializable]
    public class AutomatedFactoryCallbacks
    {
        public Action<CardFactoryRecipe> onExecutedRecipe;
        public Action<Card> onSpawnedRecipeResult;
    }
}
