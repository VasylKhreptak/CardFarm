using System;
using Quests.Data;
using UniRx;
using UnityEngine;

namespace Quests.Logic.QuestObservers.Core
{
    public abstract class QuestObserver : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] protected QuestData _questData;

        [Header("Observe Preferences")]
        [SerializeField] private bool _observeOnlyWhileActive = false;

        private IDisposable _subscription;

        #region MonoBehaviour

        private void OnValidate()
        {
            _questData ??= GetComponent<QuestData>();
        }

        private void OnEnable()
        {
            if (_observeOnlyWhileActive)
            {
                _subscription?.Dispose();
                _subscription = _questData.IsCurrentQuest.Subscribe(isActive =>
                {
                    if (isActive && _questData.IsCompletedByAction.Value == false)
                    {
                        StartObserving();
                    }
                    else
                    {
                        StopObserving();
                    }
                });
            }
            else
            {
                StartObserving();
            }
        }

        private void OnDisable()
        {
            StopObserving();
            _subscription?.Dispose();
        }

        #endregion

        public abstract void StartObserving();

        public abstract void StopObserving();

        protected void MarkQuestAsCompletedByAction(bool stopObserving = true)
        {
            _questData.IsCompletedByAction.Value = true;

            if (stopObserving)
            {
                StopObserving();
            }
        }

        protected void MarkQuestAsCompleted(bool stopObserving = true)
        {
            _questData.IsCompleted.Value = true;

            if (stopObserving)
            {
                StopObserving();
            }
        }
    }
}
