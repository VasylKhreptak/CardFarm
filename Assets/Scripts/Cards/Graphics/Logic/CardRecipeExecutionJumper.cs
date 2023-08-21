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
        private IDisposable _delaySubscription;

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

            bool canJump = isExecuting && _cardData.IsAnyGroupCardSelected.Value == false;
            
            if (canJump)
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

            _delaySubscription = Observable.Timer(TimeSpan.FromSeconds(_delay)).Subscribe(_ =>
            {
                _cardData.Animations.WaveJumpAnimation.Play(-1, onLoopPlay: TryPunchGearsScale);
            });
        }

        private void StopJumping()
        {
            _cardData.Animations.WaveJumpAnimation.Stop();
            _delaySubscription?.Dispose();
        }

        private void TryPunchGearsScale()
        {
            _gearSubscription?.Dispose();
            _gearSubscription = _cardData.GearsDrawer.Gears.Subscribe(gears =>
            {
                if (gears != null)
                {
                    gears.ScalePunchAnimation.Play();
                    _gearSubscription?.Dispose();
                }
            });
        }
    }
}
