using Cards.Data;
using ScriptableObjects.Scripts.Cards.Recipes;
using UnityEngine;

namespace Cards.Logic.Updaters
{
    public class CurrentRecipeUpdater : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;
        [SerializeField] private CardRecipes _cardRecipes;

        #region MonoBehaviour

        private void OnEnable()
        {
            _cardData.Callbacks.onBecameHeadOfGroup += OnBecameHeadOfGroup;
        }

        private void OnDisable()
        {
            _cardData.Callbacks.onBecameHeadOfGroup -= OnBecameHeadOfGroup;
            ResetCurrentRecipe();
        }

        #endregion

        private void OnBecameHeadOfGroup()
        {
            if (_cardData.IsTopCard.Value == false || _cardData.IsSingleCard.Value)
            {
                ResetCurrentRecipe();
                return;
            }

            bool hasRecipe = _cardRecipes.TryFindRecipe(_cardData.GroupCards, out CardRecipe recipe);

            if (hasRecipe == false)
            {
                ResetCurrentRecipe();
                return;
            }

            _cardData.CurrentRecipe.Value = recipe;
        }

        private void ResetCurrentRecipe()
        {
            _cardData.CurrentRecipe.Value = null;
        }
    }
}
