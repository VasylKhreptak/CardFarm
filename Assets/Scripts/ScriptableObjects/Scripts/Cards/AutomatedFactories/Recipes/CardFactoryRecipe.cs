using System;
using System.Collections.Generic;
using Cards.Core;
using ScriptableObjects.Scripts.Cards.Recipes;

namespace ScriptableObjects.Scripts.Cards.AutomatedFactories.Recipes
{
    [Serializable]
    public class CardFactoryRecipe
    {
        public List<Card> Resources = new List<Card>();
        public float Cooldown;
        public CardWeights Result = new CardWeights();
    }
}
