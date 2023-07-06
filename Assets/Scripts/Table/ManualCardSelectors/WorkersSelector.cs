using Cards.Data;
using Table.Core;

namespace Table.ManualCardSelectors
{
    public class WorkersSelector : ManualCardSelector
    {
        protected override bool IsCardAppropriate(CardData cardData)
        {
            return cardData.IsWorker;
        }
    }
}
