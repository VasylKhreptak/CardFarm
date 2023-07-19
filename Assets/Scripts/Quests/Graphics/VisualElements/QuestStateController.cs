using System;
using Quests.Data;
using Quests.Logic;
using UniRx;
using UnityEngine;
using Zenject;

namespace Quests.Graphics.VisualElements
{
    public class QuestStateController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _quest;

        private IDisposable _subscription;

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
            _subscription = _questsManager.CurrentQuest.Subscribe(UpdateQuestState);
        }

        private void StopObserving()
        {
            _subscription?.Dispose();
        }

        private void UpdateQuestState(QuestData questData)
        {
            _quest.SetActive(questData != null);
        }
    }
}
