using Cards.Data;
using Cards.Factories.Data;
using Cards.ResourceNodes.Core.Logic;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Logic.Updaters
{
    public class IsExecutingAnyRecipeUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        private FactoryData _factoryData;
        private ResourceNodeLogic _resourceNodeLogic;

        private CompositeDisposable _subscriptions = new CompositeDisposable();

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
            else if (_cardData.IsResourceNode)
            {
                _resourceNodeLogic = _cardData.GetComponentInChildren<ResourceNodeLogic>(true);
            }
        }

        private void OnEnable()
        {
            StartObservingCardData();
        }

        private void OnDisable()
        {
            StopObservingCardData();
        }

        #endregion

        private void StartObservingCardData()
        {
            StopObservingCardData();

            _cardData.RecipeExecutor.IsExecuting.Subscribe(_ => OnCardDataUpdated()).AddTo(_subscriptions);
            _cardData.ReproductionLogic.IsExecuting.Subscribe(_ => OnCardDataUpdated()).AddTo(_subscriptions);

            if (_factoryData != null)
                _factoryData.FactoryRecipeExecutor.IsExecuting.Subscribe(_ => OnCardDataUpdated()).AddTo(_subscriptions);

            if (_resourceNodeLogic != null)
                _resourceNodeLogic.IsExecuting.Subscribe(_ => OnCardDataUpdated()).AddTo(_subscriptions);
        }

        private void StopObservingCardData()
        {
            _subscriptions.Clear();
        }

        private void OnCardDataUpdated()
        {
            bool isExecutingAnyRecipe = _cardData.RecipeExecutor.IsExecuting.Value ||
                _cardData.ReproductionLogic.IsExecuting.Value;

            if (_factoryData != null)
                isExecutingAnyRecipe |= _factoryData.FactoryRecipeExecutor.IsExecuting.Value;

            if (_resourceNodeLogic != null)
                isExecutingAnyRecipe |= _resourceNodeLogic.IsExecuting.Value;


            _cardData.IsExecutingAnyRecipe.Value = isExecutingAnyRecipe;
        }
    }
}
