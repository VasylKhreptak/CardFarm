using System;
using System.Collections.Generic;
using System.Linq;
using Cards.Core;
using Cards.Data;
using Extensions;
using NaughtyAttributes;
using ScriptableObjects.Scripts.Cards.Recipes;
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

        public bool IsCompatibleByCategory(CardData topCard, CardData bottomCard)
        {
            if (IsProhibited(topCard, bottomCard)) return false;

            if (topCard.IsSellableCard && bottomCard.IsOrder) return true;

            if (topCard.IsAnimal && bottomCard.IsAutomatedFactory) return true;

            if (topCard.IsSellableCard && bottomCard.IsSellableCard) return true;

            if (topCard.IsWorker &&
                (bottomCard.IsSellableCard || bottomCard.IsAutomatedFactory)) return true;

            return IsCompatible(topCard.Card.Value, bottomCard.Card.Value);
        }

        public bool IsCompatibleByRecipe(CardData topCard, CardData bottomCard)
        {
            if (IsProhibited(topCard, bottomCard)) return false;

            if (topCard.IsSellableCard && bottomCard.IsOrder) return true;

            if (topCard.IsAnimal && bottomCard.IsAutomatedFactory) return true;

            CardData firstGroupCard = bottomCard.FirstGroupCard.Value;

            if (firstGroupCard == null) return IsCompatible(topCard.Card.Value, bottomCard.Card.Value);

            List<CardRecipe> groupPossibleRecipes = firstGroupCard.PossibleRecipes;
            List<CardData> groupCardsData = firstGroupCard.GroupCards;
            bool isCompatible = false;

            if (topCard.IsWorker == false)
            {
                foreach (var possibleRecipe in groupPossibleRecipes)
                {
                    int recipeResourcesCount = possibleRecipe.Resources.Count(x => x == topCard.Card.Value);
                    int foundResourcesCount = groupCardsData.Count(x => x.Card.Value == topCard.Card.Value);

                    isCompatible = foundResourcesCount < recipeResourcesCount;
                    if (isCompatible) break;
                }

                if (topCard.Card.Value == firstGroupCard.Card.Value &&
                    groupCardsData.Any(x => x.Card.Value != firstGroupCard.Card.Value) == false)
                {
                    return true;
                }

                if (isCompatible == false && groupPossibleRecipes.Count != 0)
                {
                    return false;
                }
            }
            else
            {
                List<Card> groupCards = groupCardsData.Select(x => x.Card.Value).ToList();

                foreach (var possibleRecipe in groupPossibleRecipes)
                {
                    if (possibleRecipe.Resources.HasExactlyAllElementsOf(groupCards) &&
                        possibleRecipe.Workers.Contains(topCard.Card.Value) &&
                        bottomCard.IsTakingPartInRecipe.Value == false)
                    {
                        isCompatible = true;
                        break;
                    }
                }
            }

            if (isCompatible)
            {
                return true;
            }

            return IsCompatible(topCard.Card.Value, bottomCard.Card.Value);
        }

        private bool IsProhibited(CardData topCard, CardData bottomCard)
        {
            if (topCard == null || bottomCard == null) return true;

            if (topCard.IsStackable == false || bottomCard.IsStackable == false) return true;

            if (bottomCard.CanBeUnderCards == false) return true;

            if (topCard.CanBeStackedOnlyWithSameCard || bottomCard.CanBeStackedOnlyWithSameCard)
            {
                if (topCard.Card.Value != bottomCard.Card.Value) return true;
            }

            if (topCard.IsAnimal && bottomCard.IsAnimal) return true;


            return false;
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
