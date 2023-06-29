using System;
using Cards.Data;
using ScriptableObjects.Scripts.Cards.Incubators.Recipes;
using UniRx;

namespace Cards.Incubators.Data
{
    [Serializable]
    public class IncubatorData : SellableCardData
    {
        public ReactiveProperty<IncubatorRecipe> CurrentIncubatorRecipe = new ReactiveProperty<IncubatorRecipe>();

        public IncubatorCallbacks IncubatorCallbacks = new IncubatorCallbacks();
    }
}
