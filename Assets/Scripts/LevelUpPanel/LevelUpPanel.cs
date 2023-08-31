using System;
using Data.Player.Core;
using Data.Player.Experience;
using LevelUpPanel.Graphics.Animations;
using NaughtyAttributes;
using UniRx;
using UnityEngine;
using Zenject;

namespace LevelUpPanel
{
    public class LevelUpPanel : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _panelObject;

        [Header("Preferences")]
        [SerializeField] private LevelUpPanelShowAnimation _showAnimation;
        [SerializeField] private LevelUpPanelHideAnimation _hideAnimation;
        [SerializeField] private float _showDelayAfterLevelUp = 1f;

        private IDisposable _levelUpSubscription;
        private IDisposable _delaySubscription;

        private ExperienceData _experienceData;

        [Inject]
        private void Constructor(PlayerData playerData)
        {
            _experienceData = playerData.ExperienceData;
        }

        #region MonoBehaviour

        private void Awake()
        {
            StartObservingLevelUp();
        }

        private void OnDestroy()
        {
            StopObservingLevelUp();
            _delaySubscription?.Dispose();
        }

        #endregion

        private void StartObservingLevelUp()
        {
            StopObservingLevelUp();

            _levelUpSubscription = _experienceData.ExperienceLevel
                .Buffer(2, 1)
                .Where(x => x[1] > x[0])
                .Subscribe(_ =>
                {
                    _delaySubscription?.Dispose();
                    _delaySubscription = Observable.Timer(TimeSpan.FromSeconds(_showDelayAfterLevelUp))
                        .Subscribe(_ => Show());
                });
        }

        private void StopObservingLevelUp()
        {
            _levelUpSubscription?.Dispose();
        }

        [Button()]
        public void Show()
        {
            _delaySubscription?.Dispose();
            Enable();
            _hideAnimation.Stop();
            _showAnimation.Play();
        }

        [Button()]
        public void Hide()
        {
            _delaySubscription?.Dispose();
            _showAnimation.Stop();
            _hideAnimation.Play(Disable);
        }

        private void Enable() => _panelObject.SetActive(true);

        private void Disable() => _panelObject.SetActive(false);

    }
}
