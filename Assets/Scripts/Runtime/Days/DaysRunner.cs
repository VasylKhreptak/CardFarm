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

        public void StartRunningDays()
        {
            StopProgress();
            StartProgress(_dayDuration);
        }

        public void StopRunningDays()
        {
            StopProgress();
        }

        protected override void OnProgressCompleted()
        {
            _daysData.Callbacks.onNewDayCome?.Invoke();
            StartProgress(_dayDuration);
        }
    }
}
