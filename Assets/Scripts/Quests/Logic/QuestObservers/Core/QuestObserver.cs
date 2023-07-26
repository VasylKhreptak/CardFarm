using Quests.Data;
using UnityEngine;

namespace Quests.Logic.QuestObservers.Core
{
    public abstract class QuestObserver : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] protected QuestData _questData;

        #region MonoBehaviour

        private void OnValidate()
        {
            _questData ??= GetComponent<QuestData>();
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

        public abstract void StartObserving();

        public abstract void StopObserving();

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
