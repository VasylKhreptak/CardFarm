using System;
using System.Collections.Generic;
using Cards.Core;
using Cards.Data;
using NaughtyAttributes;
using UnityEngine;

namespace ScriptableObjects.Scripts.Cards
{
    [CreateAssetMenu(fileName = "CompatibleCards", menuName = "ScriptableObjects/CompatibleCards")]
    public class CompatibleCards : ScriptableObject
    {
        [Header("Preferences")]
        [SerializeField] private CompatibleCardsData[] _whiteList;
        [SerializeField] private CompatibleCardsData[] _blackList;

        private Dictionary<Card, HashSet<Card>> _whiteListMap;
        private Dictionary<Card, HashSet<Card>> _blackListMap;

        public bool IsCompatible(Card topCard, Card bottomCard)
        {
            if (_whiteListMap == null || _blackListMap == null)
                Initialize();

            if (_whiteListMap.TryGetValue(topCard, out var compatibleCards))
            {
                if (compatibleCards.Contains(bottomCard)) return true;
            }

            if (_blackListMap.TryGetValue(topCard, out var incompatibleCards))
            {
                if (incompatibleCards.Contains(bottomCard)) return false;
            }

            return topCard == bottomCard;
        }

        public bool IsCompatibleByType(CardData topCard, CardData bottomCard)
        {
            if (topCard == null || bottomCard == null) return false;

            if (topCard.IsStackable == false || bottomCard.IsStackable == false) return false;

            if (topCard.CanBeStackedOnlyWithSameCard || bottomCard.CanBeStackedOnlyWithSameCard)
            {
                if (topCard.Card.Value != bottomCard.Card.Value) return false;
            }

            if (topCard.IsAnimal && bottomCard.IsAnimal) return false;

            if (topCard.IsSellableCard && bottomCard.IsOrder) return true;

            if (topCard.IsAnimal && bottomCard.IsAutomatedFactory) return true;

            if (topCard.IsSellableCard && bottomCard.IsSellableCard) return true;

            if (topCard.IsWorker &&
                (bottomCard.IsSellableCard || bottomCard.IsAutomatedFactory)) return true;

            return IsCompatible(topCard.Card.Value, bottomCard.Card.Value);
        }

        private void Awake()
        {
            Initialize();
        }

        [Button("Initialize")]
        private void Initialize()
        {
            _whiteListMap = new Dictionary<Card, HashSet<Card>>();
            _blackListMap = new Dictionary<Card, HashSet<Card>>();

            if (_whiteList == null) return;

            foreach (var whiteListCardsData in _whiteList)
            {
                _whiteListMap.Add(whiteListCardsData.Card, new HashSet<Card>(whiteListCardsData.CompatibleCards));
            }

            if (_blackList == null) return;

            foreach (var blackListCardsData in _blackList)
            {
                _blackListMap.Add(blackListCardsData.Card, new HashSet<Card>(blackListCardsData.CompatibleCards));
            }
        }

        [Serializable]
        private class CompatibleCardsData
        {
            public Card Card;
            public List<Card> CompatibleCards;
        }
    }
}
