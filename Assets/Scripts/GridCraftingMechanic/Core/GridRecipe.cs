using System;
using System.Collections.Generic;
using Cards.Core;

namespace GridCraftingMechanic.Core
{
    [Serializable]
    public class GridRecipe
    {
        public List<Card> RecipeCards = new List<Card>();
        public Card RecipeResult;
    }
}
