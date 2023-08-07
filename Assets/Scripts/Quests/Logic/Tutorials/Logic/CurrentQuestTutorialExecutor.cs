using System;
using Quests.Data;
using Quests.Logic.Tutorials.Core;
using Runtime.Commands;
using Runtime.Map;
using UniRx;
using UnityEngine;
using Zenject;

namespace Quests.Logic.Tutorials.Logic
{
    public class CurrentQuestTutorialExecutor : MonoBehaviour
    {
        private CompositeDisposable _questsSubscriptions = new CompositeDisposable();

        private IDisposable _isQuestCompletedByActionSubscription;

        private QuestTutorial _previousTutorialExecutor;

        private QuestsManager _questsManager;
        private GameRestartCommand _gameRestartCommand;
        private StarterCardsSpawner _starterCardsSpawner;

        [Inject]
        private void Constructor(QuestsManager questsManager,
            GameRestartCommand gameRestartCommand,
            StarterCardsSpawner starterCardsSpawner)
        {
            _questsManager = questsManager;
            _gameRestartCommand = gameRestartCommand;
            _starterCardsSpawner = starterCardsSpawner;
        }

        #region MonoBehaviour

        private void Awake()
        {
            OnDisable();
            _gameRestartCommand.OnExecute += OnRestart;
            _starterCardsSpawner.OnSpawnedAllCards += StartObserving;
        }

        private void OnDisable()
        {
            StopObserving();
            StopTutorial();
        }

        private void OnDestroy()
        {
            _gameRestartCommand.OnExecute -= OnRestart;
            _starterCardsSpawner.OnSpawnedAllCards -= StartObserving;
        }

        #endregion

        private void StartObserving()
        {
            _questsManager.CurrentQuest.Subscribe(_ => OnQuestsUpdated()).AddTo(_questsSubscriptions);
            _questsManager.CurrentNonRewardedQuest.Subscribe(_ => OnQuestsUpdated()).AddTo(_questsSubscriptions);
        }

        private void StopObserving()
        {
            _questsSubscriptions?.Clear();
            _isQuestCompletedByActionSubscription?.Dispose();
        }

        private void OnQuestsUpdated()
        {
            QuestData currentQuest = _questsManager.CurrentQuest.Value;

            _isQuestCompletedByActionSubscription?.Dispose();

            if (currentQuest != null && _questsManager.CurrentNonRewardedQuest.Value == null)
            {
                StartTutorial();

                _isQuestCompletedByActionSubscription = currentQuest.IsCompletedByAction.Where(x => x).Subscribe(isCompleted =>
                {
                    StopTutorial();
                });
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
        }
    }
}
