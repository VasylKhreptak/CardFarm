using System;
using Cards.Data;
using Cards.Factories.Data;
using Graphics.Animations;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Graphics.Logic
{
    public class CardRecipeExecutionJumper : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;

        [Header("Preferences")]
        [SerializeField] private float _delay = 0.5f;

        private IDisposable _gearSubscription;

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
            StopJumping();
        }

        #endregion

        private void StartObserving()
        {
            _cardData.RecipeExecutor.IsExecuting.Subscribe(_ => OnEnvironmentChanged()).AddTo(_subscriptions);
            _cardData.ReproductionLogic.IsExecuting.Subscribe(_ => OnEnvironmentChanged()).AddTo(_subscriptions);
            _cardData.IsAnyGroupCardSelected.Subscribe(_ => OnEnvironmentChanged()).AddTo(_subscriptions);
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

            if (_cardData.IsSelected.Value)
            {
                _cardData.Animations.ContinuousJumpingAnimation.StopAll();
                return;
            }

            if (_cardData.IsAnyGroupCardSelected.Value)
            {
                _cardData.Animations.ContinuousJumpingAnimation.StopContinuous();
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
            _cardData.Animations.ContinuousJumpingAnimation.PlayContinuous(delay: _delay, onPlay: TryPunchGearsScale);
        }

        private void StopJumping()
        {
            _cardData.Animations.ContinuousJumpingAnimation.StopContinuous();
            _gearSubscription?.Dispose();
        }

        private void TryPunchGearsScale()
        {
            _gearSubscription?.Dispose();
            _gearSubscription = _cardData.GearsDrawer.GearsObject.Subscribe(gears =>
            {
                if (gears != null)
                {
                    gears.GetComponentInChildren<ScalePunchAnimation>().Play();
                    _gearSubscription?.Dispose();
                }
            });
        }
    }
}
