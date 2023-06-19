using System;
using Cards.Data;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Cards.Recipes.Graphics.VisualElements
{
    public class RecipeProgress : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardData _cardData;
        [SerializeField] private Slider _slider;

        private IDisposable _progressSubscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            _slider ??= GetComponent<Slider>();
        }

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
            _progressSubscription = _cardData.RecipeExecutor.Progress.Subscribe(SetProgress);
        }

        private void StopObservingProgress()
        {
            _progressSubscription?.Dispose();
        }

        private void SetProgress(float progress)
        {
            _slider.value = progress;
        }
    }
}
