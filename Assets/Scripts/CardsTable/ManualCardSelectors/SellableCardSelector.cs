using Cards.Data;
using CardsTable.Core;

namespace CardsTable.ManualCardSelectors
{
    public class SellableCardSelector : ManualCardSelector
    {
        protected override bool IsCardAppropriate(CardDataHolder cardData)
        {
            return cardData.IsSellableCard;
        }
    }
}
