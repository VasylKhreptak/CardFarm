using System;
using Cards.Core;

namespace Quests.Data
{
    [Serializable]
    public class QuestReward
    {
        public Card[] Cards;
        public int Experience = 10;
    }
}
