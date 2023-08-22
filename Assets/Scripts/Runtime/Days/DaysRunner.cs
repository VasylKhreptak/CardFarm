using Data.Days;
using ProgressLogic.Core;
using Runtime.Commands;
using UnityEngine;
using Zenject;

namespace Runtime.Days
{
    public class DaysRunner : ProgressDependentObject
    {
        [Header("Preferences")]
        [SerializeField] private float _dayDuration = 120f;

        private DaysData _daysData;
        private GameRestartCommand _gameRestartCommand;

        [Inject]
        private void Constructor(DaysData daysData, GameRestartCommand gameRestartCommand)
        {
            _daysData = daysData;
            _gameRestartCommand = gameRestartCommand;
        }

        #region MonoBehaviour

        private void Awake()
        {
            _gameRestartCommand.OnExecute += OnRestart;
        }

        private void OnDestroy()
        {
            _gameRestartCommand.OnExecute -= OnRestart;
        }

        #endregion

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

        private void OnRestart()
        {
            _daysData.Days.Value = 1;
        }
    }
}
