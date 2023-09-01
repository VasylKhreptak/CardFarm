using System;
using Graphics.UI.Experience.Progress;
using LevelUpPanel.Graphics.Animations;
using NaughtyAttributes;
using UniRx;
using UnityEngine;

namespace LevelUpPanel
{
    public class LevelUpPanel : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _panelObject;
        [SerializeField] private UpperExperienceProgress _upperExperienceProgress;

        [Header("Preferences")]
        [SerializeField] private LevelUpPanelShowAnimation _showAnimation;
        [SerializeField] private LevelUpPanelHideAnimation _hideAnimation;

        private IDisposable _delaySubscription;

        #region MonoBehaviour

        private void Awake()
        {
            StartObservingProgress();
        }

        private void OnDestroy()
        {
            StopObservingProgress();
            _delaySubscription?.Dispose();
        }

        #endregion

        private void StartObservingProgress()
        {
            StopObservingProgress();

            _upperExperienceProgress.OnFilled += Show;
        }

        private void StopObservingProgress()
        {
            _upperExperienceProgress.OnFilled -= Show;
        }

        [Button()]
        public void Show()
        {
            _delaySubscription?.Dispose();
            Disable();
            Enable();
            _hideAnimation.Stop();
            _showAnimation.Play();
        }

        [Button()]
        public void Hide(float delay = 0f, Action onPlay = null)
        {
            _delaySubscription?.Dispose();

            _delaySubscription = Observable.Timer(TimeSpan.FromSeconds(delay))
                .Subscribe(_ =>
                {
                    _showAnimation.Stop();
                    onPlay?.Invoke();
                    _hideAnimation.Play(Disable);
                });
        }

        private void Enable() => _panelObject.SetActive(true);

        private void Disable() => _panelObject.SetActive(false);

    }
}
