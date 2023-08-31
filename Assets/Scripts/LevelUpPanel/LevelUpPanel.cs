using System;
using Data.Player.Core;
using Data.Player.Experience;
using LevelUpPanel.Buttons;
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
        [SerializeField] private WatchAddButton _watchAddButton;
        [SerializeField] private NoThanksButton _noThanksButton;

        [Header("Preferences")]
        [SerializeField] private LevelUpPanelShowAnimation _showAnimation;
        [SerializeField] private LevelUpPanelHideAnimation _hideAnimation;

        private IDisposable _levelUpSubscription;

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

            _watchAddButton.OnWatchedAd += OnWatchedAd;
        }

        private void OnDestroy()
        {
            StopObservingLevelUp();

            _watchAddButton.OnWatchedAd -= OnWatchedAd;
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
                    Show();
                });
        }

        private void StopObservingLevelUp()
        {
            _levelUpSubscription?.Dispose();
        }

        [Button()]
        public void Show()
        {
            Enable();
            _showAnimation.Play();
        }

        [Button()]
        public void Hide()
        {
            _hideAnimation.Play(Disable);
        }

        private void Enable() => _panelObject.SetActive(true);

        private void Disable() => _panelObject.SetActive(false);

        private void OnWatchedAd()
        {

        }
    }
}
