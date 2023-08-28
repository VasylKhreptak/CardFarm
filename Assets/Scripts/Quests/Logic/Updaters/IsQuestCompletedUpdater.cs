using System;
using Quests.Data;
using UniRx;
using UnityEngine;
using UnlockedCardPanel.Graphics.VisualElements;
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

        private NewCardPanel _newCardPanel;

        [Inject]
        private void Constructor(NewCardPanel newCardPanel)
        {
            _newCardPanel = newCardPanel;
        }

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

            _questData.IsCompletedByAction.Subscribe(_ => OnDataUpdated()).AddTo(_questDataSubscriptions);
            _newCardPanel.IsActive.Subscribe(_ => OnDataUpdated()).AddTo(_questDataSubscriptions);
        }

        private void StopObservingData()
        {
            _questDataSubscriptions?.Clear();
        }

        private void OnDataUpdated()
        {
            bool isCompletedByAction = _questData.IsCompletedByAction.Value;
            bool isNewPanelActive = _newCardPanel.IsActive.Value;

            if (isCompletedByAction && isNewPanelActive == false)
            {
                StopObservingData();
                MarkQuestAsCompletedDelayed();
            }
        }

        private void MarkQuestAsCompletedDelayed()
        {
            _delaySubscription?.Dispose();
            _delaySubscription = Observable
                .Timer(TimeSpan.FromSeconds(_completeDelay))
                .Subscribe(_ => MarkQuestAsCompleted());
        }

        private void MarkQuestAsCompleted(bool stopObserving = true)
        {            
            _questData.IsCompleted.Value = true;

            if (stopObserving)
            {
                StopObservingData();
            }
        }
    }
}
