using Quests.Data;
using Quests.Logic.Tutorials.Core;
using Runtime.Commands;
using UniRx;
using UnityEngine;
using Zenject;

namespace Quests.Logic.Tutorials.Logic
{
    public class CurrentQuestTutorialExecutor : MonoBehaviour
    {
        private CompositeDisposable _questsSubscriptions = new CompositeDisposable();

        private QuestTutorial _previousTutorialExecutor;

        private QuestsManager _questsManager;
        private GameRestartCommand _gameRestartCommand;

        [Inject]
        private void Constructor(QuestsManager questsManager, GameRestartCommand gameRestartCommand)
        {
            _questsManager = questsManager;
            _gameRestartCommand = gameRestartCommand;
        }

        #region MonoBehaviour

        private void Awake()
        {
            _gameRestartCommand.OnExecute += OnRestart;
        }

        private void OnEnable()
        {
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
            StopTutorial();
        }

        private void OnDestroy()
        {
            _gameRestartCommand.OnExecute -= OnRestart;
        }

        #endregion

        private void StartObserving()
        {
            _questsManager.CurrentQuest.Subscribe(_ => OnQuestsUpdated()).AddTo(_questsSubscriptions);
            _questsManager.CurrentNonRewardedQuest.Subscribe(_ => OnQuestsUpdated()).AddTo(_questsSubscriptions);
        }

        private void StopObserving()
        {
            _questsSubscriptions.Clear();
        }

        private void OnQuestsUpdated()
        {
            if (_questsManager.CurrentQuest.Value != null && _questsManager.CurrentNonRewardedQuest.Value == null)
            {
                StartTutorial();
            }
            else
            {
                StopTutorial();
            }
        }

        private void StartTutorial()
        {
            StopTutorial();

            QuestData currentQuest = _questsManager.CurrentQuest.Value;

            if (currentQuest == null) return;

            QuestTutorial tutorialExecutor = _questsManager.CurrentQuest.Value.Tutorial;

            if (tutorialExecutor == null) return;

            tutorialExecutor.StartTutorial();

            _previousTutorialExecutor = tutorialExecutor;
        }

        private void StopTutorial()
        {
            if (_previousTutorialExecutor != null)
            {
                _previousTutorialExecutor.StopTutorial();
            }
        }

        private void OnRestart()
        {
            OnDisable();

            if (gameObject.activeSelf)
            {
                OnEnable();
            }
        }
    }
}
