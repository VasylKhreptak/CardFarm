using Cards.Core;
using Cards.Data;
using Data.Cards.Core;
using GridCraftingMechanic.Cards.GridCells;
using GridCraftingMechanic.Core;
using ScriptableObjects.Scripts.Cards.Data;
using UnityEngine;
using Zenject;

namespace GridCraftingMechanic.Cards.Grid.Logic.Updaters
{
    public class GridRecipeApplier : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CraftingGridCardData _cardData;

        private CardsData _cardsData;

        [Inject]
        private void Constructor(CardsData cardsData)
        {
            _cardsData = cardsData;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<CraftingGridCardData>(true);
        }

        private void OnEnable()
        {
            ApplyRecipe(_cardData.GridRecipe.Value);
        }

        #endregion

        private void ApplyRecipe(GridRecipe gridRecipe)
        {
            if (gridRecipe == null) return;

            int length = Mathf.Min(gridRecipe.RecipeCards.Count, _cardData.GridCells.Count);

            for (int i = 0; i < length; i++)
            {
                Card recipeTargetCard = gridRecipe.RecipeCards[i];
                GridCellCardData gridCell = _cardData.GridCells[i];

                gridCell.TargetCard.Value = recipeTargetCard;
                gridCell.ContainsTargetCard.Value = false;

                ApplyAppearance(gridCell, recipeTargetCard);
            }

            ApplyAppearance(_cardData, gridRecipe.RecipeResult);
        }

        private void ApplyAppearance(CardData cardData, Card card)
        {
            if (_cardsData.TryGetValue(card, out CardDataHolder cardDataHolder))
            {
                _cardData.Name.Value = cardDataHolder.Name;
                _cardData.Icon.Value = cardDataHolder.Icon;
            }
        }
    }
}
