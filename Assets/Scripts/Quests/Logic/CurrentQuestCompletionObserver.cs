using System;
using Quests.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Quests.Logic
{
    public class CurrentQuestCompletionObserver : MonoBehaviour
    {
        private BoolReactiveProperty _isCurrentQuestCompleted = new BoolReactiveProperty(false);

        public IReadOnlyReactiveProperty<bool> IsCurrentQuestCompleted => _isCurrentQuestCompleted;

        private IDisposable _currentQuestSubscription;
        private IDisposable _isQuestCompletedSubscription;

        private QuestsManager _questsManager;

        [Inject]
        private void Constructor(QuestsManager questsManager)
        {
            _questsManager = questsManager;
        }

        #region MonoBehaviour

        private void OnEnable()
        {
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
        }

        #endregion

        private void StartObserving()
        {
            StartObservingCurrentQuest();
        }

        private void StopObserving()
        {
            StopObservingCurrentQuest();
            StopObservingQuestCompletion();
            
        }

        private void StartObservingCurrentQuest()
        {
            _currentQuestSubscription = _questsManager.CurrentQuest.Subscribe(OnCurrentQuestChanged);
        }

        private void StopObservingCurrentQuest()
        {
            _currentQuestSubscription?.Dispose();
        }

        private void OnCurrentQuestChanged(QuestData questData)
        {
            if (questData == null)
            {
                StopObservingQuestCompletion();
            }
            else
            {
                StartObservingQuestCompletion(questData);
            }
        }

        private void StartObservingQuestCompletion(QuestData questData)
        {
            StopObservingQuestCompletion();
            _isQuestCompletedSubscription = questData.IsCompleted.Subscribe(isCompleted =>
            {
                _isCurrentQuestCompleted.Value = isCompleted;
            });
        }

        private void StopObservingQuestCompletion()
        {
            _isQuestCompletedSubscription?.Dispose();
            _isCurrentQuestCompleted.Value = false;
        }
    }
}
