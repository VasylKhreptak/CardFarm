using System;
using Cards.Core;
using ScriptableObjects.Scripts.Cards.Incubators.Recipes;

namespace Cards.Incubators.Data
{
    [Serializable]
    public class IncubatorCallbacks
    {
        public Action<IncubatorRecipe> onExecutedRecipe;
        public Action<Card> onSpawnedResultedCard;
    }
}
