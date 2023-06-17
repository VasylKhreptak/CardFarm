using System.Collections.Generic;
using Cards.Core;
using NaughtyAttributes;
using ScriptableObjects.Scripts.Cards.Recipes;
using UnityEngine;

public class RecipesTest : MonoBehaviour
{
    [Header("Preferences")]
    [SerializeField] private List<Card> _recipeCards;
    [SerializeField] private CardRecipes _recipes;

    public CardRecipe FoundRecipe;

    [Button]
    private void FindRecipe()
    {
        // if (_recipes.IsRecipeValid(_recipeCards, out CardRecipe recipe))
        // {
        //     FoundRecipe = recipe;
        // }
    }
}
