using System;
using Cards.Zones.BuyZone.Data;
using Quests.Data;
using Quests.Logic;
using Quests.Logic.Core;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cards.Zones.BuyZone.Logic.Updaters
{
    public class BuyZoneLockedStateController : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private BuyZoneData _buyZoneData;

        [Header("Preferences")]
        [SerializeField] private Quest _targetQuest;

        private QuestsManager _questsManager;

        private IDisposable _questAddSubscription;
        private IDisposable _questCompletionSubscription;

        [Inject]
        private void Constructor(QuestsManager questsManager)
        {
            _questsManager = questsManager;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _buyZoneData = GetComponentInParent<BuyZoneData>(true);
        }

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

            if (_questsManager.TryGetQuestData(_targetQuest, out QuestData quest))
            {
                StartObservingQuestCompletion(quest);
            }
            else
            {
                _questAddSubscription = _questsManager.TotalQuests.ObserveAdd().Subscribe(addEvent =>
                {
                    if (addEvent.Value.Quest == _targetQuest)
                    {
                        StartObservingQuestCompletion(addEvent.Value);
                        _questAddSubscription?.Dispose();
                    }
                });
            }
        }

        private void StartObservingQuestCompletion(QuestData quest)
        {
            StopObservingQuestCompletion();
            _questCompletionSubscription = quest.IsCompleted.Subscribe(isCompleted =>
            {
                _buyZoneData.IsLocked.Value = !isCompleted;
            });
        }

        private void StopObservingQuestCompletion()
        {
            _questCompletionSubscription?.Dispose();
        }

        private void StopObserving()
        {
            _questAddSubscription?.Dispose();
            _questCompletionSubscription?.Dispose();
        }
    }
}
