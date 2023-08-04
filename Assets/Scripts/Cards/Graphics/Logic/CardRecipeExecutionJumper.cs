using Cards.Data;
using Cards.Factories.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Graphics.Logic
{
    public class CardRecipeExecutionJumper : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        private CompositeDisposable _recipeSubscriptions = new CompositeDisposable();

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<CardData>(true);
        }

        private void OnEnable()
        {
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
        }

        #endregion

        private void StartObserving()
        {
            _cardData.RecipeExecutor.IsExecuting.Subscribe(OnIsExecutingValueChanged).AddTo(_recipeSubscriptions);
            _cardData.ReproductionLogic.IsExecuting.Subscribe(OnIsExecutingValueChanged).AddTo(_recipeSubscriptions);

            if (_cardData.IsAutomatedFactory)
            {
                FactoryData factoryData = _cardData as FactoryData;

                if (factoryData == null) return;

                factoryData.FactoryRecipeExecutor.IsExecuting.Subscribe(OnIsExecutingValueChanged).AddTo(_recipeSubscriptions);
            }
        }

        private void StopObserving()
        {
            _recipeSubscriptions?.Clear();
        }

        private void OnIsExecutingValueChanged(bool isExecuting)
        {
            if (isExecuting)
            {
                StartJumping();
            }
            else
            {
                StopJumping();
            }
        }

        private void StartJumping()
        {
            StopJumping();
            _cardData.Animations.ContinuousJumpingAnimation.PlayContinuous();
        }

        private void StopJumping()
        {
            _cardData.Animations.ContinuousJumpingAnimation.StopContinuous();
        }
    }
}
