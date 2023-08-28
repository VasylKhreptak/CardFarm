using System;
using System.Collections.Generic;
using Cards.Core;
using ScriptableObjects.Scripts.Cards.Recipes;
using Random = UnityEngine.Random;

namespace ScriptableObjects.Scripts.Cards.AutomatedFactories.Recipes
{
    [Serializable]
    public class FactoryRecipe
    {
        public List<Card> Resources = new List<Card>();
        public bool RemoveResourcesOnComplete = true;
        public float Cooldown;
        public int MinResultCount = 1;
        public int MaxResultCount = 1;
        public CardWeights Result = new CardWeights();

        public int EnergyCost = 1;

        public int ResultCount => Random.Range(MinResultCount, MaxResultCount + 1);
    }
}
