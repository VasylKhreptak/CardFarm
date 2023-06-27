using System;
using Quests.Data;
using Quests.Logic;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Quests.Graphics.VisualElements
{
    public class CurrentQuestName : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_Text _tmp;

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
            StopObserving();
            _currentQuestSubscription = _questsManager.CurrentQuest.Subscribe(OnCurrentQuestUpdated);
        }

        private void StopObserving()
        {
            _currentQuestSubscription?.Dispose();
            StopObservingIfCompleted();
        }

        private void OnCurrentQuestUpdated(QuestData quest)
        {
            if (quest == null)
            {
                _tmp.text = string.Empty;
                StopObservingIfCompleted();
                return;
            }

            _tmp.text = quest.Name;
            StartObservingIfCompleted();
        }

        private void StartObservingIfCompleted()
        {
            StopObservingIfCompleted();

            _isQuestCompletedSubscription = _questsManager.CurrentQuest.Value.IsCompleted
                .Subscribe(OnCurrentQuestCompletedValueChanged);
        }

        private void StopObservingIfCompleted()
        {
            _isQuestCompletedSubscription?.Dispose();
        }

        private void OnCurrentQuestCompletedValueChanged(bool isCompleted)
        {
            if (isCompleted)
            {
                _tmp.text = _questsManager.CurrentQuest.Value.Name + " Completed!";
            }
        }
    }
}
