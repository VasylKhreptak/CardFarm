using Data.Days;
using ProgressLogic.Core;
using UnityEngine;
using Zenject;

namespace Runtime.Days
{
    public class DaysRunner : ProgressDependentObject
    {
        [Header("Preferences")]
        [SerializeField] private float _dayDuration = 120f;

        private DaysData _daysData;

        [Inject]
        private void Constructor(DaysData daysData)
        {
            _daysData = daysData;
        }

        #region MonoBehaviour

        private void Start()
        {
            StartRunningDays();
        }

        #endregion

        private void StartRunningDays()
        {
            StopProgress();
            RunDay();
        }

        private void RunDay()
        {
            StartProgress(_dayDuration);
        }

        protected override void OnProgressCompleted()
        {
            _daysData.Callbacks.onNewDayCome?.Invoke();
            RunDay();
        }
    }
}
