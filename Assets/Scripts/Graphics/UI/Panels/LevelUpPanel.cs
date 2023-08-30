using System;
using Data.Player.Core;
using Data.Player.Experience;
using Graphics.Animations.LevelUpPanelAnimations;
using UniRx;
using UnityEngine;
using Zenject;

namespace Graphics.UI.Panels
{
    public class LevelUpPanel : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _panelObject;

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
        }

        private void OnDestroy()
        {
            StopObservingLevelUp();
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
                    Debug.Log("Level Upo");
                });
        }

        private void StopObservingLevelUp()
        {
            _levelUpSubscription?.Dispose();
        }
    }
}
