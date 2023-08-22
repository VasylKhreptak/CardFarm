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

        private bool _isRunning;

        public void StartRunningDays()
        {
            StopProgress();
            StartProgress(_dayDuration);
            _isRunning = true;
        }

        public void StopRunningDays()
        {
            StopProgress();
            _isRunning = false;
        }

        protected override void OnProgressCompleted()
        {
            _daysData.Callbacks.onNewDayCome?.Invoke();
            StartProgress(_dayDuration);
        }

        private void OnRestart()
        {
            _daysData.Days.Value = 1;
            StopRunningDays();
        }
    }
}
