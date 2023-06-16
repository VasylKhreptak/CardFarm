using System;
using System.Collections.Generic;
using Cards.Core;

namespace ScriptableObjects.Scripts.Cards.Recipes
{
    [Serializable]
    public class CardRecipe
    {
        public List<Card> Cards = new List<Card>();
        public float Cooldown;
        public CardWeights Result = new CardWeights();
    }
}
