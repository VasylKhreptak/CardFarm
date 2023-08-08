using Cards.Data;
using CardsTable.Core;

namespace CardsTable.ManualCardSelectors
{
    public class SellableCardSelector : ManualCardSelector
    {
        protected override bool IsCardAppropriate(CardData cardData)
        {
            return cardData.IsSellableCard;
        }
    }
}
