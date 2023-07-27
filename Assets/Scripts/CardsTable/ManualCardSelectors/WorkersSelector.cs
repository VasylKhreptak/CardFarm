﻿using Cards.Data;
using CardsTable.Core;

namespace CardsTable.ManualCardSelectors
{
    public class WorkersSelector : ManualCardSelector
    {
        protected override bool IsCardAppropriate(CardData cardData)
        {
            return cardData.IsWorker;
        }
    }
}