using System;
using System.Collections.Generic;
using Cards.Core;
using NaughtyAttributes;
using Random = UnityEngine.Random;

namespace ScriptableObjects.Scripts.Cards.Recipes
{
    [Serializable]
    public class CardRecipe
    {
        public List<Card> Resources = new List<Card>();
        public List<Card> Workers = new List<Card>();
        public float Cooldown;
        public int MinResultCount = 1;
        public int MaxResultCount = 1;
        public CardWeights Result = new CardWeights();
        public bool UsePseudoRandom = false;
        [ShowIf(nameof(UsePseudoRandom))] public Card FirstCard;

        public bool HasWorkers => Workers.Count > 0;
        public int ResultCount => Random.Range(MinResultCount, MaxResultCount + 1);
    }
}
