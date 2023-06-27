﻿using System;
using System.Collections.Generic;
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

        private ReactiveCollection<QuestData> _quests = new ReactiveCollection<QuestData>();
        private ReactiveProperty<QuestData> _currentQuest = new ReactiveProperty<QuestData>();

        private IDisposable _questsCountSubscription;

        public IReadOnlyReactiveCollection<QuestData> Quests => _quests;
        public IReadOnlyReactiveProperty<QuestData> CurrentQuest => _currentQuest;

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
            _quests.Add(questData);
        }

        public void UnregisterQuest(QuestData questData)
        {
            _quests.Remove(questData);
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
            _questsCountSubscription = _quests.ObserveCountChanged().Subscribe(OnQuestsCountUpdated);
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
            if (_quests.Count == 0)
            {
                questData = null;
                return false;
            }

            foreach (var possibleQuest in _quests)
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
    }
}