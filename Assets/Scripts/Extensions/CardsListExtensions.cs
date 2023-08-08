using System.Collections.Generic;
using System.Linq;
using Cards.Core;
using Cards.Data;

namespace Extensions
{
    public static class CardsListExtensions
    {
        public static bool TryGetResources(this List<CardDataHolder> cards, out List<CardDataHolder> resources)
        {
            List<CardDataHolder> possibleResources = cards
                .Where(x => x.IsWorker == false)
                .ToList();

            resources = possibleResources;
            return possibleResources.Count > 0;
        }

        public static bool TryGetResources(this List<CardDataHolder> cards, out List<Card> resources)
        {
            bool result = TryGetResources(cards, out List<CardDataHolder> possibleResources);
            resources = possibleResources.Select(x => x.Card.Value).ToList();
            return result;
        }

        public static bool TryGetWorkers(this List<CardDataHolder> cards, out List<CardDataHolder> workers)
        {
            List<CardDataHolder> possibleWorkers = cards
                .Where(x => x.IsWorker)
                .ToList();

            workers = possibleWorkers;
            return possibleWorkers.Count > 0;
        }

        public static bool TryGetWorkers(this List<CardDataHolder> cards, out List<Card> workers)
        {
            bool result = TryGetWorkers(cards, out List<CardDataHolder> possibleWorkers);
            workers = possibleWorkers.Select(x => x.Card.Value).ToList();
            return result;
        }
    }
}
