using Quests.Data;
using UnityEngine;
using Zenject;

namespace Quests.Logic.QuestObservers.Core
{
    public abstract class QuestObserver : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] protected QuestData _questData;

        private QuestsManager _questsManager;

        [Inject]
        private void Constructor(QuestsManager questsManager)
        {
            _questsManager = questsManager;
        }

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
            _questsManager.UnregisterQuest(_questData);
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
