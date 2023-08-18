using Data.Days;
using Graphics.Animations.Quests.QuestPanel;
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
        private QuestShowAnimation _questShowAnimation;

        [Inject]
        private void Constructor(DaysData daysData,
            GameRestartCommand gameRestartCommand,
            QuestShowAnimation questShowAnimation)
        {
            _daysData = daysData;
            _gameRestartCommand = gameRestartCommand;
            _questShowAnimation = questShowAnimation;
        }

        private bool _isRunning;

        #region MonoBehaviour

        private void Awake()
        {
            _gameRestartCommand.OnExecute += OnRestart;
            _questShowAnimation.OnCompleted += OnCompletedShowAnimation;
        }

        private void OnDestroy()
        {
            _gameRestartCommand.OnExecute -= OnRestart;
            _questShowAnimation.OnCompleted -= OnCompletedShowAnimation;
        }

        #endregion

        private void OnCompletedShowAnimation()
        {
            if (_isRunning == false)
            {
                StartRunningDays();
            }
        }

        private void StartRunningDays()
        {
            StopProgress();
            StartProgress(_dayDuration);
            _isRunning = true;
        }

        private void StopRunningDays()
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
