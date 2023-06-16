using System;
using System.Collections.Generic;
using Cards.Core;
using NaughtyAttributes;
using UnityEngine;

namespace ScriptableObjects.Scripts.Cards
{
    [CreateAssetMenu(fileName = "CompatibleCards", menuName = "ScriptableObjects/CompatibleCards")]
    public class CompatibleCards : ScriptableObject
    {
        [Header("Preferences")]
        [SerializeField] private CompatibleCardsData[] _compatibleCardsData;

        private Dictionary<Card, HashSet<Card>> _compatibleCards;

        public bool IsCompatible(Card topCard, Card bottomCard)
        {
            if (_compatibleCards == null)
                Initialize();

            if (_compatibleCards.TryGetValue(topCard, out var compatibleCards))
            {
                return compatibleCards.Contains(bottomCard);
            }

            return false;
        }

        private void Awake()
        {
            Initialize();
        }

        [Button("Initialize")]
        private void Initialize()
        {
            _compatibleCards = new Dictionary<Card, HashSet<Card>>();

            if (_compatibleCardsData == null) return;

            foreach (var compatibleCardsData in _compatibleCardsData)
            {
                _compatibleCards.Add(compatibleCardsData.Card, new HashSet<Card>(compatibleCardsData.CompatibleCards));
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
