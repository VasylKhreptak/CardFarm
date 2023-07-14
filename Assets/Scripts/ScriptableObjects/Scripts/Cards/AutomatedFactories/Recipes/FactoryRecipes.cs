using System.Collections.Generic;
using System.Linq;
using Cards.Core;
using Cards.Data;
using Extensions;
using UnityEngine;

namespace ScriptableObjects.Scripts.Cards.AutomatedFactories.Recipes
{
    [CreateAssetMenu(fileName = "FactoryRecipes", menuName = "ScriptableObjects/FactoryRecipes")]
    public class FactoryRecipes : ScriptableObject
    {
        [Header("References")]
        [SerializeField] private List<FactoryRecipe> _recipes;

        public bool TryFindRecipe(List<Card> resources, out FactoryRecipe recipe)
        {
            if (resources.Count == 0)
            {
                recipe = null;
                return false;
            }

            foreach (var possibleRecipe in _recipes)
            {
                int recipeResourcesCount = possibleRecipe.Resources.Count;

                List<Card> clampedResources = resources.GetRange(0, Mathf.Min(resources.Count, recipeResourcesCount));

                if (clampedResources.Count < recipeResourcesCount) continue;
                
                if (clampedResources.HasExactlyAllElementsOf(possibleRecipe.Resources))
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
