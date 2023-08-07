using System;
using Cards.Graphics.Logic.ProgressHeightUpdaters.Core;
using UniRx;
using Zenject;

namespace Cards.Graphics.Logic
{
    public class CardProgressHeightUnlinker : BaseCardProgressHeightUnlinker, IValidatable
    {
        private IDisposable _isRecipeExecutingSubscription;

        #region MonoBehaviour

        private void OnEnable()
        {
            StartObserving();
        }

        protected override void OnDisable()
        {
            StopObserving();

            base.OnDisable();
        }

        #endregion

        private void StartObserving()
        {
            StopObserving();

            _isRecipeExecutingSubscription = _cardData.RecipeExecutor.IsExecuting.Subscribe(isExecuting =>
            {
                if (isExecuting)
                {
                    StartUnlinking();
                }
                else
                {
                    StopUnlinking();
                }
            });
        }

        private void StopObserving()
        {
            _isRecipeExecutingSubscription?.Dispose();
        }
    }
}
