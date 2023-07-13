using Cards.AutomatedFactories.Logic;
using Cards.Data;

namespace Cards.AutomatedFactories.BreakableFactories.Logic
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
