using System.Collections.Generic;
using System.Linq;
using Cards.Core;
using Cards.Data;

namespace Extensions
{
    public static class CardsListExtensions
    {
        public static bool TryGetResources(this List<CardData> cards, out List<CardData> resources)
        {
            List<CardData> possibleResources = cards
                .Where(x => x.IsWorker == false)
                .ToList();

            resources = possibleResources;
            return possibleResources.Count > 0;
        }

        public static bool TryGetResources(this List<CardData> cards, out List<Card> resources)
        {
            bool result = TryGetResources(cards, out List<CardData> possibleResources);
            resources = possibleResources.Select(x => x.Card.Value).ToList();
            return result;
        }

        public static bool TryGetWorkers(this List<CardData> cards, out List<CardData> workers)
        {
            List<CardData> possibleWorkers = cards
                .Where(x => x.IsWorker)
                .ToList();

            workers = possibleWorkers;
            return possibleWorkers.Count > 0;
        }

        public static bool TryGetWorkers(this List<CardData> cards, out List<Card> workers)
        {
            bool result = TryGetWorkers(cards, out List<CardData> possibleWorkers);
            workers = possibleWorkers.Select(x => x.Card.Value).ToList();
            return result;
        }
    }
}
