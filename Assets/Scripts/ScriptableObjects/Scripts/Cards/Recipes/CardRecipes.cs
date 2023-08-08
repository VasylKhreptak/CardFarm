using System.Collections.Generic;
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

        public bool TryFindRecipe(List<CardDataHolder> cards, out CardRecipe recipe)
        {
            bool hasWorkers = cards.TryGetWorkers(out List<Card> workers);
            bool hasResources = cards.TryGetResources(out List<Card> resources);
            int workersCount = workers.Count;

            // if (resources.IsAllElementsSame())
            // {
            //     resources.Resize(1);
            // }

            foreach (var possibleRecipe in _recipes)
            {
                List<Card> recipeResources = possibleRecipe.Resources;
                int recipeWorkersCount = possibleRecipe.Workers.Count;

                if (recipeWorkersCount != workersCount) continue;

                if (hasResources == false) continue;

                if (recipeResources.HasExactlyAllElementsOf(resources))
                {
                    recipe = possibleRecipe;
                    return true;
                }
            }

            recipe = null;
            return false;
        }

        public bool TryGetPossibleRecipes(List<CardDataHolder> cards, out List<CardRecipe> possibleRecipes)
        {
            List<CardRecipe> foundRecipes = new List<CardRecipe>();

            bool hasResources = cards.TryGetResources(out List<Card> resources);
            bool hasWorkers = cards.TryGetWorkers(out List<Card> workers);
            int workersCount = workers.Count;

            // if (resources.IsAllElementsSame())
            // {
            //     resources.Resize(1);
            // }

            foreach (var possibleRecipe in _recipes)
            {
                List<Card> recipeResources = possibleRecipe.Resources;
                int recipeWorkersCount = possibleRecipe.Workers.Count;

                if (workersCount > recipeWorkersCount) continue;

                if (recipeResources.HasAllLessElementOf(resources))
                {
                    foundRecipes.Add(possibleRecipe);
                }
            }

            possibleRecipes = foundRecipes;
            return foundRecipes.Count > 0;
        }
    }
}
