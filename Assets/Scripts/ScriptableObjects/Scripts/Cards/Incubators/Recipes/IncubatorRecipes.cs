using System.Collections.Generic;
using Cards.Core;
using Extensions;
using UnityEngine;

namespace ScriptableObjects.Scripts.Cards.Incubators.Recipes
{
    [CreateAssetMenu(fileName = "IncubatorRecipes", menuName = "ScriptableObjects/IncubatorRecipes")]
    public class IncubatorRecipes : ScriptableObject
    {
        [Header("References")]
        [SerializeField] private List<IncubatorRecipe> _recipes;

        public bool TryFindRecipe(List<Card> resources, out IncubatorRecipe recipe)
        {
            if (resources.Count == 0)
            {
                recipe = null;
                return false;
            }

            foreach (var possibleRecipe in _recipes)
            {
                if (possibleRecipe.Resources.Count != resources.Count) continue;

                if (resources.HasExactlyAllElementsOf(possibleRecipe.Resources))
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
