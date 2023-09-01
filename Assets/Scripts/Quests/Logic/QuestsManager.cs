using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
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

        private ReactiveCollection<QuestData> _totalQuests = new ReactiveCollection<QuestData>();
        private ReactiveProperty<QuestData> _currentQuest = new ReactiveProperty<QuestData>();
        private ReactiveProperty<QuestData> _currentNonRewardedQuest = new ReactiveProperty<QuestData>();
        private ReactiveCollection<QuestData> _completedQuests = new ReactiveCollection<QuestData>();
        private ReactiveCollection<QuestData> _notCompletedQuests = new ReactiveCollection<QuestData>();
        private ReactiveCollection<QuestData> _rewardedQuests = new ReactiveCollection<QuestData>();
        private ReactiveCollection<QuestData> _nonRewardedQuests = new ReactiveCollection<QuestData>();

        private CompositeDisposable _questStateSubscriptions = new CompositeDisposable();

        public IReadOnlyReactiveCollection<QuestData> TotalQuests => _totalQuests;
        public IReadOnlyReactiveProperty<QuestData> CurrentQuest => _currentQuest;
        public IReadOnlyReactiveProperty<QuestData> CurrentNonRewardedQuest => _currentNonRewardedQuest;
        public IReadOnlyReactiveCollection<QuestData> CompletedQuests => _completedQuests;
        public IReadOnlyReactiveCollection<QuestData> NotCompletedQuests => _notCompletedQuests;
        public IReadOnlyReactiveCollection<QuestData> RewardedQuests => _rewardedQuests;
        public IReadOnlyReactiveCollection<QuestData> NonRewardedQuests => _nonRewardedQuests;

        private QuestsObjectPooler _questsObjectPooler;

        [Inject]
        private void Constructor(QuestsObjectPooler questsObjectPooler)
        {
            _questsObjectPooler = questsObjectPooler;
        }

        #region MonoBehaviour

        private void Awake()
        {
            CreateQuests();
            StartObservingTotalQuests();
        }
        private void OnDestroy()
        {
            StopObservingTotalQuests();
        }

        #endregion

        private void CreateQuests()
        {
            foreach (var questToRegister in _questsToRegister)
            {
                QuestData questData = _questsObjectPooler.Spawn(questToRegister).GetComponent<QuestData>();

                _totalQuests.Add(questData);
            }
        }

        private void StartObservingTotalQuests()
        {
            StopObservingTotalQuests();

            foreach (var quest in _totalQuests)
            {
                quest.IsCompleted.Subscribe(_ => OnQuestUpdated(quest)).AddTo(_questStateSubscriptions);
                quest.TookReward.Subscribe(_ => OnQuestUpdated(quest)).AddTo(_questStateSubscriptions);
            }
        }

        private void OnQuestUpdated(QuestData quest)
        {
            if (quest.IsCompleted.Value)
            {
                if (_completedQuests.Contains(quest) == false)
                {
                    _completedQuests.Add(quest);
                }

                _notCompletedQuests.Remove(quest);

                if (quest.TookReward.Value)
                {
                    if (_rewardedQuests.Contains(quest) == false)
                    {
                        _rewardedQuests.Add(quest);
                    }

                    _nonRewardedQuests.Remove(quest);
                }
                else
                {
                    _rewardedQuests.Remove(quest);

                    if (_nonRewardedQuests.Contains(quest) == false)
                    {
                        _nonRewardedQuests.Add(quest);
                    }
                }
            }
            else
            {
                _completedQuests.Remove(quest);

                if (_notCompletedQuests.Contains(quest) == false)
                {
                    _notCompletedQuests.Add(quest);
                }
                _rewardedQuests.Remove(quest);
                _nonRewardedQuests.Remove(quest);
            }

            UpdateCurrentNonRewardedQuest();
            UpdateCurrentQuest();
            MaskQuestAsCurrent(_currentQuest.Value);
        }

        private void UpdateCurrentQuest()
        {
            QuestData quest = null;

            foreach (var possibleQuest in _totalQuests)
            {
                if (possibleQuest.IsCompleted.Value == false && possibleQuest.TookReward.Value == false)
                {
                    quest = possibleQuest;
                    break;
                }
            }

            _currentQuest.Value = quest;
        }

        private void UpdateCurrentNonRewardedQuest()
        {
            _currentNonRewardedQuest.Value = _nonRewardedQuests.FirstOrDefault();
        }

        private void StopObservingTotalQuests()
        {
            _questStateSubscriptions.Clear();
        }

        public bool TryGetQuestData(Quest quest, out QuestData questData)
        {
            questData = null;

            foreach (var possibleQuestData in _totalQuests)
            {
                if (possibleQuestData.Quest == quest)
                {
                    questData = possibleQuestData;
                    return true;
                }
            }

            return false;
        }

        private void MaskQuestAsCurrent(QuestData quest)
        {
            foreach (var possibleQuest in _totalQuests)
            {
                possibleQuest.IsCurrentQuest.Value = possibleQuest == quest;
            }
        }

        [Button()]
        public void PauseQuests()
        {
            StopObservingTotalQuests();

            _currentQuest.Value = null;
            _currentNonRewardedQuest.Value = null;
        }

        [Button()]
        public void ResumeQuests()
        {
            StartObservingTotalQuests();
        }
    }
}
