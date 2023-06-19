using Cards.Data;
using ProgressLogic.Core;
using UnityEngine;

namespace Cards.Recipes
{
    public class RecipeExecutor : ProgressDependentObject
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        protected override void OnProgressCompleted()
        {

        }
    }
}
