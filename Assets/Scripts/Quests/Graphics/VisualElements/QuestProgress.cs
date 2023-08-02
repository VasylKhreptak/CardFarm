using System;
using Quests.Data;
using Quests.Logic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Quests.Graphics.VisualElements
{
    public class QuestProgress : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Slider _slider;

        private IDisposable _currentQuestSubscription;
        private IDisposable _progressSubscription;

        private QuestsManager _questsManager;

        [Inject]
        private void Constructor(QuestsManager questsManager)
        {
            _questsManager = questsManager;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _slider ??= GetComponent<Slider>();
        }

        private void OnEnable()
        {
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
            StopObservingProgress();
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
                StopObservingProgress();
                return;
            }

            StartObservingProgress(quest);
        }

        private void StartObservingProgress(QuestData quest)
        {
            StopObservingProgress();

            _progressSubscription = quest.Progress.Subscribe(SetProgress);
        }

        private void StopObservingProgress()
        {
            _progressSubscription?.Dispose();
        }

        private void SetProgress(float progress)
        {
            _slider.value = progress;
        }
    }
}
