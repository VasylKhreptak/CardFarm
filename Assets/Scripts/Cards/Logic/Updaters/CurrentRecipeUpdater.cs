using Cards.Data;
using Cards.Workers.Data;
using Extensions;
using ScriptableObjects.Scripts.Cards.Recipes;
using UniRx;
using UnityEngine;

namespace Cards.Logic.Updaters
{
    public class CurrentRecipeUpdater : RecipeUpdaterCore<CardData>
    {
        [Header("References")]
        [SerializeField] private CardRecipes _cardRecipes;

        private CompositeDisposable _workerSubscriptions = new CompositeDisposable();

        #region MonoBehaviour

        protected override void OnDisable()
        {
            base.OnDisable();

            _workerSubscriptions.Clear();
        }

        #endregion

        protected override void ResetCurrentRecipe()
        {
            _cardData.CurrentRecipe.Value = null;
        }

        protected override void OnBecameHeadOfGroup()
        {
            base.OnBecameHeadOfGroup();

            _workerSubscriptions.Clear();

            _cardRecipes.TryFindRecipe(_cardData.GroupCards, out CardRecipe recipe);

            if (recipe == null)
            {
                ResetCurrentRecipe();
                return;
            }

            int recipeEnergyCost = recipe.EnergyCost;

            foreach (var groupCard in _cardData.GroupCards)
            {
                if (groupCard is WorkerData worker)
                {
                    if (worker.Energy.Value < recipeEnergyCost)
                    {
                        worker.Energy.Subscribe(workerEnergy =>
                        {
                            if (_cardData.GroupCards.HasEnergy(recipeEnergyCost))
                            {
                                _workerSubscriptions.Clear();
                                UpdateRecipe();
                            }
                        }).AddTo(_workerSubscriptions);
                    }
                }
            }
        }

        protected override void UpdateRecipe()
        {
            _cardRecipes.TryFindRecipe(_cardData.GroupCards, out CardRecipe recipe);

            if (recipe == null || _cardData.GroupCards.HasEnergy(recipe.EnergyCost) == false)
            {
                ResetCurrentRecipe();
                return;
            }

            _cardData.CurrentRecipe.Value = recipe;
        }
    }
}
