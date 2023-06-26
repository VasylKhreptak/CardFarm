using System;
using ScriptableObjects.Scripts.Cards.Recipes;

namespace ScriptableObjects.Scripts.Cards.ResourceNodes
{
    [Serializable]
    public class ResourceNodeRecipe
    {
        public float Cooldown;
        public CardWeights Result = new CardWeights();
    }
}
