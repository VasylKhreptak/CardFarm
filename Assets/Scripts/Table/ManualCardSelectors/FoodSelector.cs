using Cards.Data;
using Table.Core;

namespace Table.ManualCardSelectors
{
    public class FoodSelector : ManualCardSelector
    {
        protected override bool IsCardAppropriate(CardData cardData)
        {
            return cardData.IsFood;
        }
    }
}
