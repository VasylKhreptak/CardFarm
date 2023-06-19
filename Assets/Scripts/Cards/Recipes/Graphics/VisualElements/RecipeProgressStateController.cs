using System;
using Cards.Data;
using UniRx;
using UnityEngine;

namespace Cards.Recipes.Graphics.VisualElements
{
    public class RecipeProgressStateController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;
        [SerializeField] private GameObject _progressObject;

        private IDisposable _progressSubscription;

        #region MonoBehaviour

        private void OnEnable()
        {
            StartObservingProgress();
        }

        private void OnDisable()
        {
            StopObservingProgress();
        }

        #endregion

        private void StartObservingProgress()
        {
            StopObservingProgress();
            _progressSubscription = _cardData.RecipeExecutor.Progress.Subscribe(OnProgressChanged);
        }

        private void StopObservingProgress()
        {
            _progressSubscription?.Dispose();
        }

        private void OnProgressChanged(float progress)
        {
            _progressObject.SetActive(progress > 0);
        }
    }
}
