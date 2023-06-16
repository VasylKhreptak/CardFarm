using System;
using Cards.Core;

namespace ScriptableObjects.Scripts.Cards.Recipes
{
    [Serializable]
    public class CardRecipe
    {
        public Card[] Cards;
        public float Cooldown;
        public CardWeights Result = new CardWeights();
    }
}
