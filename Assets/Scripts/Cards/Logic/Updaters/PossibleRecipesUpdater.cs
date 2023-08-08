using System.Collections.Generic;
using Cards.Data;
using ScriptableObjects.Scripts.Cards.Recipes;
using UnityEngine;

namespace Cards.Logic.Updaters
{
    public class PossibleRecipesUpdater : PossibleRecipesUpdaterCore<CardDataHolder>
    {
        [Header("References")]
        [SerializeField] private CardRecipes _cardRecipes;

        protected override void ResetPossibleRecipes()
        {
            _cardData.PossibleRecipes.Clear();
        }

        protected override void UpdatePossibleRecipes()
        {
            ResetPossibleRecipes();
            
            if (_cardRecipes.TryGetPossibleRecipes(_cardData.GroupCards, out List<CardRecipe> recipes))
            {
                _cardData.PossibleRecipes.AddRange(recipes);
            }
        }
    }
}
