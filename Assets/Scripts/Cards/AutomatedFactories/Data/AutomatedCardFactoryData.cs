using Cards.Data;
using ScriptableObjects.Scripts.Cards.AutomatedFactories.Recipes;
using UniRx;

namespace Cards.AutomatedFactories.Data
{
    public class AutomatedCardFactoryData : SellableCardData
    {
        public ReactiveProperty<FactoryRecipe> CurrentFactoryRecipe = new ReactiveProperty<FactoryRecipe>();

        public AutomatedFactoryCallbacks AutomatedFactoryCallbacks = new AutomatedFactoryCallbacks();
    }
}
