using System.Collections.Generic;
using System.Linq;
using ObjectPoolers;
using Quests.Core;
using Quests.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Quests.Logic
{
    public class QuestsManager : MonoBehaviour
    {
        [Header("Preferences")]
        [SerializeField] private List<Quest> _questsToRegister = new List<Quest>();

        private ReactiveCollection<QuestData> _quests = new ReactiveCollection<QuestData>();
        private ReactiveProperty<QuestData> _currentQuest = new ReactiveProperty<QuestData>();

        public IReadOnlyReactiveCollection<QuestData> Quests => _quests;
        public IReadOnlyReactiveProperty<QuestData> CurrentQuest => _currentQuest;

        private QuestsObjectPooler _questsObjectPooler;

        [Inject]
        private void Constructor(QuestsObjectPooler questsObjectPooler)
        {
            _questsObjectPooler = questsObjectPooler;
        }

        #region MonoBehaviour

        private void Awake()
        {
            InitQuests();
            StartObserving();
        }

        private void OnDestroy()
        {
            StopObserving();
        }

        #endregion

        private void InitQuests()
        {
            foreach (var questToRegister in _questsToRegister)
            {
                QuestData questData = _questsObjectPooler.Spawn(questToRegister).GetComponent<QuestData>();
                
                _quests.Add(questData);
            }
        }

        private void StartObserving()
        {
            _currentQuest.Value = _quests.First();
        }

        private void StopObserving()
        {

        }
    }
}
