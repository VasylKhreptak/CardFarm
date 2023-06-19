using System;
using System.Collections.Generic;
using Cards.Core;

namespace ScriptableObjects.Scripts.Cards.Recipes
{
    [Serializable]
    public class CardRecipe
    {
        public List<Card> Resources = new List<Card>();
        public List<Card> Workers = new List<Card>();
        public float Cooldown;
        public CardWeights Result = new CardWeights();
    }
}
