using Cards.Data;
using CardsTable.Core;

namespace CardsTable.ManualCardSelectors
{
    public class FoodSelector : ManualCardSelector
    {
        protected override bool IsCardAppropriate(CardData cardData)
        {
            return cardData.IsFood;
        }
    }
}
