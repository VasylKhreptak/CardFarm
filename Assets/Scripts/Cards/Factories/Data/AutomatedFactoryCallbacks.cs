using System;
using Cards.Core;
using ScriptableObjects.Scripts.Cards.AutomatedFactories.Recipes;

namespace Cards.Factories.Data
{
    [Serializable]
    public class AutomatedFactoryCallbacks
    {
        public Action<FactoryRecipe> onExecutedRecipe;
    }
}
