using Cards.AutomatedFactories.Logic;
using Cards.Data;

namespace Cards.AutomatedFactories.BreakableFactories.Logic
{
    public class BreakableFactoryRecipeExecutor : FactoryRecipeExecutor
    {
        protected override void OnProgressCompleted()
        {
            base.OnProgressCompleted();

            _cardData.Durability.Value--;
        }

        protected override void TryLinkCardToTop(CardData card)
        {
            if (_cardData.Durability.Value <= 1) return;

            base.TryLinkCardToTop(card);
        }
    }
}
