using Cards.Factories.Logic;

namespace Cards.Factories.BreakableFactories.Logic
{
    public class BreakableFactoryRecipeExecutor : FactoryRecipeExecutor
    {
        protected override void OnProgressCompleted()
        {
            base.OnProgressCompleted();
            
            if (_cardData.IsBreakable)
            {
                _cardData.Durability.Value--;
            }
        }
    }
}
