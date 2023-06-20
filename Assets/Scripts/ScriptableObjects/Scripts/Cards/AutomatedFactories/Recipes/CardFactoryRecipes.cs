using System.Collections.Generic;
using System.Linq;
using Cards.Core;
using Cards.Data;
using Extensions;
using UnityEngine;

namespace ScriptableObjects.Scripts.Cards.AutomatedFactories.Recipes
{
    [CreateAssetMenu(fileName = "New Recipe", menuName = "ScriptableObjects/CardFactoryRecipes")]
    public class CardFactoryRecipes : ScriptableObject
    {
        [Header("References")]
        [SerializeField] private List<CardFactoryRecipe> _recipes;

        public bool TryFindRecipe(List<CardData> resources, out CardFactoryRecipe recipe)
        {
            if (resources.Count == 0)
            {
                recipe = null;
                return false;
            }

            foreach (var possibleRecipe in _recipes)
            {
                int recipeResourcesCount = possibleRecipe.Resources.Count;

                List<CardData> clampedResources = resources.GetRange(0, recipeResourcesCount);

                if (clampedResources.Count < recipeResourcesCount) continue;

                List<Card> clampedResourcesCards = clampedResources.Select(x => x.Card.Value).ToList();

                if (clampedResourcesCards.HasAllElementsOf(possibleRecipe.Resources))
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
