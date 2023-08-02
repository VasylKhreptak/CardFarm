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

        private CompositeDisposable _subscriptions = new CompositeDisposable();

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
            _questsManager.CurrentQuest.Subscribe(_ => TryUpdateName()).AddTo(_subscriptions);
            _questsManager.CurrentNonRewardedQuest.Subscribe(_ => TryUpdateName()).AddTo(_subscriptions);
        }

        private void StopObserving()
        {
            _subscriptions?.Clear();
        }

        private void TryUpdateName()
        {
            QuestData currentQuest = _questsManager.CurrentQuest.Value;
            QuestData nonRewardedQuest = _questsManager.CurrentNonRewardedQuest.Value;

            if (currentQuest == null || nonRewardedQuest != null) return;

            _tmp.text = currentQuest.Name;
        }
    }
}
