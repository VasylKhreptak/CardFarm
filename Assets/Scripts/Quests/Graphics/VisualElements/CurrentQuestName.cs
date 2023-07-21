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
        }

        private void OnCurrentQuestUpdated(QuestData quest)
        {
            if (quest == null)
            {
                _tmp.text = string.Empty;
                return;
            }

            _tmp.text = quest.Name;
        }
    }
}
