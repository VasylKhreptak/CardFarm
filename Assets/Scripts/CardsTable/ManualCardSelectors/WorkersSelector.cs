using Cards.Data;
using CardsTable.Core;

namespace CardsTable.ManualCardSelectors
{
    public class WorkersSelector : ManualCardSelector
    {
        protected override bool IsCardAppropriate(CardDataHolder cardData)
        {
            return cardData.IsWorker;
        }
    }
}
