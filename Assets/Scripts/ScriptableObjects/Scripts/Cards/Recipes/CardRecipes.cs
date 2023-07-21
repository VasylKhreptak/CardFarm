using System.Collections.Generic;
using System.Linq;
using Cards.Core;
using Cards.Data;
using Extensions;
using UnityEngine;

namespace ScriptableObjects.Scripts.Cards.Recipes
{
    [CreateAssetMenu(fileName = "New Recipe", menuName = "ScriptableObjects/CardRecipes")]
    public class CardRecipes : ScriptableObject
    {
        [Header("Preferences")]
        [SerializeField] private List<CardRecipe> _recipes;

        public bool TryFindRecipe(List<CardData> cards, out CardRecipe recipe)
        {
            bool hasResources = cards.TryGetResources(out List<Card> resources);
            bool hasWorkers = cards.TryGetWorkers(out List<Card> workers);

            if (hasResources == false)
            {
                recipe = null;
                return false;
            }

            if (hasWorkers == false)
            {
                foreach (var possibleRecipe in _recipes)
                {
                    if (resources.HasExactlyAllElementsOf(possibleRecipe.Resources) && possibleRecipe.HasWorkers == false)
                    {
                        recipe = possibleRecipe;
                        return true;
                    }
                }
            }

            foreach (var possibleRecipe in _recipes)
            {
                if (possibleRecipe.Workers.Count == workers.Count && resources.HasExactlyAllElementsOf(possibleRecipe.Resources))
                {
                    recipe = possibleRecipe;
                    return true;
                }
            }

            Card firstResource = resources.First();
            if (resources.All(x => x == firstResource))
            {
                foreach (var possibleRecipe in _recipes)
                {
                    if (possibleRecipe.Resources.Count == 1
                        && possibleRecipe.Resources.First() == firstResource
                        && possibleRecipe.Workers.Count == workers.Count)
                    {
                        recipe = possibleRecipe;
                        return true;
                    }
                }
            }

            recipe = null;
            return false;
        }

        public bool TryGetPossibleRecipes(List<CardData> cards, out List<CardRecipe> possibleRecipes)
        {
            List<CardRecipe> foundRecipes = new List<CardRecipe>();

            bool hasResources = cards.TryGetResources(out List<Card> resources);

            if (hasResources == false)
            {
                possibleRecipes = null;
                return false;
            }

            Card firstResource = resources.First();
            
            bool isResourcesAllSame = resources.Any(x => x != firstResource) == false;
            
            foreach (var recipe in _recipes)
            {
                if(isResourcesAllSame && recipe.Resources.Count == 1 && recipe.Resources.First() == firstResource)
                {
                    foundRecipes.Add(recipe);
                    continue;
                }
                
                if (recipe.Resources.HasAllLessElementOf(resources))
                {
                    foundRecipes.Add(recipe);
                }
            }

            possibleRecipes = foundRecipes;
            return foundRecipes.Count > 0;
        }
    }
}
