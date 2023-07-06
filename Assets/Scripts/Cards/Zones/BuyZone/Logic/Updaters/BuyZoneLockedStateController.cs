using Cards.Zones.BuyZone.Data;
using Quests.Logic;
using Quests.Logic.Core;
using UnityEngine;
using Zenject;

namespace Cards.Zones.BuyZone.Logic.Updaters
{
    public class BuyZoneLockedStateController : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private BuyZoneData _buyZoneData;

        [Header("Preferences")]
        [SerializeField] private Quest _targetQuest;

        private QuestsManager _questsManager;

        [Inject]
        private void Constructor(QuestsManager questsManager)
        {
            _questsManager = questsManager;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _buyZoneData = GetComponentInParent<BuyZoneData>(true);
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

        private void StartObserving()
        {
            StopObserving();

            _buyZoneData.IsLocked.Value = _questsManager.IsQuestFinished(_targetQuest) == false;
            _questsManager.onStartedQuest += OnStartedQuest;
            _questsManager.onFinishedQuest += OnFinishedQuest;
        }

        private void StopObserving()
        {
            _questsManager.onStartedQuest -= OnStartedQuest;
            _questsManager.onFinishedQuest -= OnFinishedQuest;
        }

        private void OnStartedQuest(Quest quest)
        {
            if (quest != _targetQuest) return;

            _buyZoneData.IsLocked.Value = true;
        }

        private void OnFinishedQuest(Quest quest)
        {
            if (quest != _targetQuest) return;

            _buyZoneData.IsLocked.Value = false;
        }
    }
}
