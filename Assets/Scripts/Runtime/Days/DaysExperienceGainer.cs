using Data.Days;
using Data.Player.Core;
using Data.Player.Experience;
using UnityEngine;
using Zenject;

namespace Runtime.Days
{
    public class DaysExperienceGainer : MonoBehaviour
    {
        [Header("Preferences")]
        [SerializeField] private int _experienceOnDayPassed = 100;

        private DaysData _daysData;
        private ExperienceData _experienceData;

        [Inject]
        private void Constructor(DaysData daysData, PlayerData playerData)
        {
            _daysData = daysData;
            _experienceData = playerData.ExperienceData;
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
            _daysData.Callbacks.onNewDayCome += GainExperience;
        }

        private void StopObserving()
        {
            _daysData.Callbacks.onNewDayCome -= GainExperience;
        }

        private void GainExperience()
        {
            _experienceData.TotalExperience.Value += _experienceOnDayPassed;
        }
    }
}
