using System;
using Quests.Data;
using Quests.Logic;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Quests.Graphics.VisualElements
{
    public class QuestQuantityText : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_Text _tmp;

        [Header("Preferences")]
        [SerializeField] private string _format = "{0}/{1}";

        private CompositeDisposable _questDataSubscriptions = new CompositeDisposable();

        private IDisposable _questSubscription;

        private QuestsManager _questsManager;

        [Inject]
        private void Constructor(QuestsManager questsManager)
        {
            _questsManager = questsManager;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _tmp ??= GetComponent<TMP_Text>();
        }

        private void OnEnable()
        {
            StartObserving();
        }

        private void OnDisable()
        {
            StopObserving();
            _questDataSubscriptions?.Clear();
        }

        #endregion

        private void StartObserving()
        {
            StopObserving();

            _questSubscription = _questsManager.CurrentQuest.Subscribe(OnQuestChanged);
        }

        private void StopObserving()
        {
            _questSubscription?.Dispose();
        }

        private void OnQuestChanged(QuestData questData)
        {
            _questDataSubscriptions?.Clear();

            if (questData == null) return;

            questData.CurrentQuantity.Subscribe(_ => OnQuestDataUpdated(questData)).AddTo(_questDataSubscriptions);
            questData.TargetQuantity.Subscribe(_ => OnQuestDataUpdated(questData)).AddTo(_questDataSubscriptions);
        }

        private void OnQuestDataUpdated(QuestData questData)
        {
            _tmp.text = string.Format(_format, questData.CurrentQuantity.Value, questData.TargetQuantity.Value);
        }
    }
}
