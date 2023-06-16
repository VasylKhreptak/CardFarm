using System.Collections.Generic;
using System.Linq;
using Cards.Core;
using UnityEngine;

namespace ScriptableObjects.Scripts.Cards.Recipes
{
    [CreateAssetMenu(fileName = "New Recipe", menuName = "ScriptableObjects/CardRecipes")]
    public class CardRecipes : ScriptableObject
    {
        [Header("Preferences")]
        [SerializeField] private List<CardRecipe> _recipes;

        public bool IsRecipeValid(List<Card> cards, out CardRecipe recipe)
        {
            HashSet<Card> cardsss = new HashSet<Card>();

            foreach (var possibleRecipe in _recipes)
            {
                if (cards.SequenceEqual(possibleRecipe.Cards))
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
