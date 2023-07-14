using System.Collections.Generic;
using Cards.Core;
using Extensions;
using UnityEngine;

namespace ScriptableObjects.Scripts.Cards.ReproductionRecipes
{
    [CreateAssetMenu(fileName = "ReproductionRecipes", menuName = "ScriptableObjects/ReproductionRecipes")]
    public class CardReproductionRecipes : ScriptableObject
    {
        [Header("Preferences")]
        [SerializeField] private List<CardReproductionRecipe> _recipes = new List<CardReproductionRecipe>();

        public bool TryGetRecipe(List<Card> cards, out CardReproductionRecipe recipe)
        {
            if (cards.Count == 0)
            {
                recipe = null;
                return false;
            }

            foreach (var possibleRecipe in _recipes)
            {
                if (cards.HasExactlyAllElementsOf(possibleRecipe.Resources))
                {
                    recipe = possibleRecipe;
                    return true;
                }
            }

            recipe = null;
            return false;
        }
    }
}
