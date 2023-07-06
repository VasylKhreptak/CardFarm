using System;
using System.Collections.Generic;
using System.Linq;
using ObjectPoolers;
using Quests.Data;
using Quests.Logic.Core;
using UniRx;
using UnityEngine;
using Zenject;

namespace Quests.Logic
{
    public class QuestsManager : MonoBehaviour
    {
        [Header("Preferences")]
        [SerializeField] private List<Quest> _questsToRegister = new List<Quest>();

        private ReactiveCollection<QuestData> _leftQuests = new ReactiveCollection<QuestData>();
        private ReactiveProperty<QuestData> _currentQuest = new ReactiveProperty<QuestData>();
        private ReactiveCollection<Quest> _finishedQuests = new ReactiveCollection<Quest>();

        private IDisposable _questsCountSubscription;

        public IReadOnlyReactiveCollection<QuestData> LeftQuests => _leftQuests;
        public IReadOnlyReactiveProperty<QuestData> CurrentQuest => _currentQuest;
        public IReadOnlyReactiveCollection<Quest> FinishedQuests => _finishedQuests;

        public event Action<Quest> onFinishedQuest;
        public event Action<Quest> onStartedQuest;

        private QuestsObjectPooler _questsObjectPooler;

        [Inject]
        private void Constructor(QuestsObjectPooler questsObjectPooler)
        {
            _questsObjectPooler = questsObjectPooler;
        }

        #region MonoBehaviour

        private void Awake()
        {
            StartObservingQuestsCount();
            CreateQuests();
        }

        private void OnDestroy()
        {
            StopObservingQuestsCount();
        }

        #endregion

        public void RegisterQuest(QuestData questData)
        {
            _leftQuests.Add(questData);
            _finishedQuests.Remove(questData.Quest);
            onStartedQuest?.Invoke(questData.Quest);
        }

        public void UnregisterQuest(QuestData questData)
        {
            _leftQuests.Remove(questData);
            _finishedQuests.Add(questData.Quest);
            onFinishedQuest?.Invoke(questData.Quest);
        }

        private void CreateQuests()
        {
            foreach (var questToRegister in _questsToRegister)
            {
                QuestData questData = _questsObjectPooler.Spawn(questToRegister).GetComponent<QuestData>();

                RegisterQuest(questData);
            }
        }

        private void StartObservingQuestsCount()
        {
            StopObservingQuestsCount();
            _questsCountSubscription = _leftQuests.ObserveCountChanged().Subscribe(OnQuestsCountUpdated);
        }

        private void StopObservingQuestsCount()
        {
            _questsCountSubscription?.Dispose();
        }

        private void OnQuestsCountUpdated(int questsCount)
        {
            UpdateCurrentQuest();
        }

        private void UpdateCurrentQuest()
        {
            TryGetFirstNonRewardedQuest(out QuestData questData);
            _currentQuest.Value = questData;
        }

        private bool TryGetFirstNonRewardedQuest(out QuestData questData)
        {
            if (_leftQuests.Count == 0)
            {
                questData = null;
                return false;
            }

            foreach (var possibleQuest in _leftQuests)
            {
                if (possibleQuest.TookReward.Value == false)
                {
                    questData = possibleQuest;
                    return true;
                }
            }

            questData = null;
            return false;
        }

        public bool IsQuestFinished(Quest quest)
        {
            return _finishedQuests.Contains(quest);
        }

        public bool IsQuestActive(Quest quest)
        {
            QuestData questData = _leftQuests.FirstOrDefault(q => q.Quest == quest);

            return questData != null;
        }
    }
}
