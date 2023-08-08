using Cards.Data;
using Cards.Factories.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Graphics.Logic
{
    public class RecipeExecutionItemDrawer : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardDataHolder _cardData;

        [Header("Preferences")]
        [SerializeField] private GameObject _targetObject;

        private CompositeDisposable _subscriptions = new CompositeDisposable();

        private FactoryData _factoryData;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _cardData = GetComponentInParent<CardDataHolder>(true);
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
            _cardData.IsAnyGroupCardSelected.Subscribe(_ => OnEnvironmentChanged()).AddTo(_subscriptions);

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

            if (_cardData.IsAnyGroupCardSelected.Value)
            {
                Hide();
                return;
            }

            if (isExecuting)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        private void Show()
        {
            _targetObject.SetActive(true);
        }

        private void Hide()
        {
            _targetObject.SetActive(false);
        }
    }
}
