using System;
using System.Collections.Generic;
using Cards.Core;
using ScriptableObjects.Scripts.Cards.Recipes;

namespace ScriptableObjects.Scripts.Cards.ReproductionRecipes
{
    [Serializable]
    public class CardReproductionRecipe
    {
        public float Cooldown;
        public List<Card> Resources = new List<Card>();
        public List<Card> ResourcesToRemove = new List<Card>();
        public List<CardWeight> Results = new List<CardWeight>();
    }
}
