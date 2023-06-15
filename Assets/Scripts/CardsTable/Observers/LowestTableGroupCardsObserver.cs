using System.Collections.Generic;
using System.Linq;
using Cards.Data;
using UniRx;
using UnityEngine;

namespace CardsTable.Observers
{
    public class LowestTableGroupCardsObserver : MonoBehaviour
    {
        private ReactiveCollection<CardData> _lowestCards = new ReactiveCollection<CardData>();

        public IReadOnlyReactiveCollection<CardData> LowestCards => _lowestCards;

        public List<CardData> LowestCardsList => _lowestCards.ToList();
    }
}
