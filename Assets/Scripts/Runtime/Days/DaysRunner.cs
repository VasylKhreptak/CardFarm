using Data.Days;
using NaughtyAttributes;
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

        private void Start()
        {
            StartRunningDays();
        }

        private void OnDestroy()
        {
            _gameRestartCommand.OnExecute -= OnRestart;
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

        [Button("Trigger New Day")]
        protected override void OnProgressCompleted()
        {
            _daysData.Callbacks.onNewDayCome?.Invoke();
            RunDay();
        }

        private void OnRestart()
        {
            _daysData.Days.Value = 1;
            StartRunningDays();
        }
    }
}
