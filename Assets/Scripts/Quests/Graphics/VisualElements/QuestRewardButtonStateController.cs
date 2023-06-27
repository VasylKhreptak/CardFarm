using System;
using Quests.Data;
using Quests.Logic;
using UniRx;
using UnityEngine;
using Zenject;

namespace Quests.Graphics.VisualElements
{
    public class QuestRewardButtonStateController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _buttonObject;

        private IDisposable _currentQuestSubscription;
        private IDisposable _isQuestCompletedSubscription;

        private QuestsManager _questsManager;

        [Inject]
        private void Constructor(QuestsManager questsManager)
        {
            _questsManager = questsManager;
        }

        #region Monobehaviour

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
            StopObserving();
            StartObservingCurrentQuest();
        }

        private void StopObserving()
        {
            StopObservingCurrentQuest();
            StopObservingIsQuestCompleted();
            _buttonObject.SetActive(false);
        }

        private void StartObservingCurrentQuest()
        {
            StopObservingCurrentQuest();
            _currentQuestSubscription = _questsManager.CurrentQuest.Subscribe(OnCurrentQuestUpdated);
        }

        private void StopObservingCurrentQuest()
        {
            _currentQuestSubscription?.Dispose();
            StopObservingIsQuestCompleted();
        }

        private void StartObservingIsQuestCompleted()
        {
            StopObservingIsQuestCompleted();
            _isQuestCompletedSubscription = _questsManager.CurrentQuest.Value.IsCompleted.Subscribe(OnIsQuestCompletedUpdated);
        }

        private void StopObservingIsQuestCompleted()
        {
            _isQuestCompletedSubscription?.Dispose();
            _buttonObject.SetActive(false);
        }

        private void OnCurrentQuestUpdated(QuestData quest)
        {
            if (quest != null)
            {
                StartObservingIsQuestCompleted();
            }
        }

        private void OnIsQuestCompletedUpdated(bool isCompleted)
        {
            _buttonObject.SetActive(isCompleted);
        }
    }
}
