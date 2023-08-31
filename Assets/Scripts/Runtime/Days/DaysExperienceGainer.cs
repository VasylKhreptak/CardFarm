using Data.Days;
using Data.Player.Core;
using Data.Player.Experience;
using Extensions;
using Quests.Logic;
using Quests.Logic.Core;
using UnityEngine;
using Zenject;

namespace Runtime.Days
{
    public class DaysExperienceGainer : MonoBehaviour
    {
        [Header("Preferences")]
        [SerializeField] private int _experienceOnDayPassed = 100;

        [Header("Preferences")]
        [SerializeField] private Quest _targetQuest;

        private DaysData _daysData;
        private ExperienceData _experienceData;
        private QuestsManager _questsManager;

        [Inject]
        private void Constructor(DaysData daysData,
            PlayerData playerData,
            QuestsManager questsManager)
        {
            _daysData = daysData;
            _experienceData = playerData.ExperienceData;
            _questsManager = questsManager;
        }

        #region MonoBehaviour

        private void Awake()
        {
            StartObserving();
        }

        private void OnDestroy()
        {
            StopObserving();
        }

        #endregion

        private void StartObserving()
        {
            StopObserving();
            _daysData.Callbacks.onNewDayCome += TryGainExperience;
        }

        private void StopObserving()
        {
            _daysData.Callbacks.onNewDayCome -= TryGainExperience;
        }

        private void TryGainExperience()
        {
            if (_questsManager.CompletedQuests.Contains(x => x.Quest == _targetQuest))
            {
                _experienceData.TotalExperience.Value += _experienceOnDayPassed;
            }
        }
    }
}
