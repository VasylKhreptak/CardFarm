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
                    if (resources.HasAllElementsOf(possibleRecipe.Resources) && possibleRecipe.HasWorkers == false)
                    {
                        recipe = possibleRecipe;
                        return true;
                    }
                }
            }

            foreach (var possibleRecipe in _recipes)
            {
                if (possibleRecipe.Workers.HasAllElementsOf(workers) && resources.HasAllElementsOf(possibleRecipe.Resources))
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
                        && possibleRecipe.Workers.HasAllElementsOf(workers))
                    {
                        recipe = possibleRecipe;
                        return true;
                    }
                }
            }

            recipe = null;
            return false;
        }
    }
}
