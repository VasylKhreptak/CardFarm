using System;
using Quests.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Quests.Logic.Updaters
{
    public class IsQuestCompletedUpdater : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private QuestData _questData;

        [Header("Preferences")]
        [SerializeField] private float _completeDelay = 0.8f;

        private IDisposable _delaySubscription;

        private CompositeDisposable _questDataSubscriptions = new CompositeDisposable();

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _questData = GetComponentInParent<QuestData>(true);
        }

        private void OnEnable()
        {
            StartObservingData();

        }

        private void OnDisable()
        {
            StopObservingData();
            _delaySubscription?.Dispose();
        }

        #endregion

        private void StartObservingData()
        {
            StopObservingData();

            _questData.Progress.Subscribe(_ => OnDataUpdated()).AddTo(_questDataSubscriptions);
            _questData.IsCompletedByAction.Subscribe(_ => OnDataUpdated()).AddTo(_questDataSubscriptions);
        }

        private void StopObservingData()
        {
            _questDataSubscriptions?.Clear();
        }

        private void OnDataUpdated()
        {
            bool isCompletedByAction = _questData.IsCompletedByAction.Value;

            if (isCompletedByAction == false) return;

            float progress = _questData.Progress.Value;

            if (Mathf.Approximately(progress, 1f))
            {
                StopObservingData();
                CompleteQuestDelayed();
            }
        }

        private void CompleteQuestDelayed()
        {
            _delaySubscription?.Dispose();
            _delaySubscription = Observable
                .Timer(TimeSpan.FromSeconds(_completeDelay))
                .Subscribe(_ => CompleteQuest());
        }

        private void CompleteQuest()
        {
            _questData.IsCompleted.Value = true;
        }
    }
}
