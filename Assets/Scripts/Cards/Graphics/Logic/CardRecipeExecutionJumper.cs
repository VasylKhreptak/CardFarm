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

        private CompositeDisposable _subscriptions = new CompositeDisposable();

        private FactoryData _factoryData;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<CardData>(true);
        }

        private void Awake()
        {
            if (_cardData.IsAutomatedFactory)
            {
                _factoryData = _cardData as FactoryData;
            }
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
            _cardData.RecipeExecutor.IsExecuting.Subscribe(_ => OnEnvironmentChanged()).AddTo(_subscriptions);
            _cardData.ReproductionLogic.IsExecuting.Subscribe(_ => OnEnvironmentChanged()).AddTo(_subscriptions);
            _cardData.IsSelected.Subscribe(_ => OnEnvironmentChanged()).AddTo(_subscriptions);

            if (_factoryData != null)
            {
                _factoryData.FactoryRecipeExecutor.IsExecuting.Subscribe(_ => OnEnvironmentChanged()).AddTo(_subscriptions);
            }
        }

        private void StopObserving()
        {
            _subscriptions?.Clear();
        }

        private void OnEnvironmentChanged()
        {
            bool isExecuting = _cardData.RecipeExecutor.IsExecuting.Value || _cardData.ReproductionLogic.IsExecuting.Value;

            if (_factoryData != null && _factoryData.FactoryRecipeExecutor.IsExecuting.Value)
            {
                isExecuting = true;
            }

            bool isSelected = _cardData.IsSelected.Value;

            if (isSelected)
            {
                _cardData.Animations.ContinuousJumpingAnimation.StopAll();
                return;
            }

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
