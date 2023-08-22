using CBA.Animations.Sequences.Core;
using Graphics.Animations.Quests.QuestPanel;
using Runtime.Commands;
using Runtime.Days;
using UnityEngine;
using Zenject;

namespace Graphics.UI.Panels
{
    public class DaysProgressPanel : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _panelObject;

        [Header("Preferences")]
        [SerializeField] private AnimationSequence _showAnimation;

        private DaysRunner _daysRunner;
        private QuestShowAnimation _questShowAnimation;
        private GameRestartCommand _gameRestartCommand;

        [Inject]
        private void Constructor(DaysRunner daysRunner,
            QuestShowAnimation questShowAnimation,
            GameRestartCommand gameRestartCommand)
        {
            _daysRunner = daysRunner;
            _questShowAnimation = questShowAnimation;
            _gameRestartCommand = gameRestartCommand;
        }

        #region MonoBehaviour

        private void Awake()
        {
            OnRestart();

            _questShowAnimation.OnCompleted += OnCompletedShowAnimation;
            _gameRestartCommand.OnExecute += OnRestart;
        }

        private void OnDestroy()
        {
            _questShowAnimation.OnCompleted -= OnCompletedShowAnimation;
            _gameRestartCommand.OnExecute -= OnRestart;
        }

        #endregion

        private void OnCompletedShowAnimation()
        {
            _panelObject.SetActive(true);
            _showAnimation.Stop();
            _showAnimation.PlayFromStartImmediate();
            _daysRunner.StartRunningDays();            
        }

        private void OnRestart()
        {
            _showAnimation.Stop();
            _panelObject.SetActive(false);
            _daysRunner.StopRunningDays();
        }
    }
}
