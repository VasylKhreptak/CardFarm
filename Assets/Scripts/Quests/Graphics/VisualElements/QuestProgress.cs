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

        private CompositeDisposable _questSubscriptions = new CompositeDisposable();
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
            _questsManager.CurrentQuest.Subscribe(_ => OnQuestDataUpdated()).AddTo(_questSubscriptions);
            _questsManager.CurrentNonRewardedQuest.Subscribe(_ => OnQuestDataUpdated()).AddTo(_questSubscriptions);
        }

        private void StopObserving()
        {
            _questSubscriptions?.Clear();
        }

        private void OnQuestDataUpdated()
        {
            QuestData currentQuest = _questsManager.CurrentQuest.Value;
            QuestData nonRewardedQuest = _questsManager.CurrentNonRewardedQuest.Value;

            if (currentQuest == null || nonRewardedQuest != null)
            {
                StopObservingProgress();
                return;
            }

            StartObservingProgress(currentQuest);
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
