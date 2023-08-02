using System;
using Quests.Logic.Core;
using Quests.Logic.Tutorials.Core;
using UniRx;
using UnityEngine;
using Zenject;

namespace Quests.Data
{
    [Serializable]
    public class QuestData : MonoBehaviour, IValidatable
    {
        public Quest Quest;
        public string Name;
        public QuestReward Reward;
        public BoolReactiveProperty IsCompleted = new BoolReactiveProperty();
        public BoolReactiveProperty TookReward = new BoolReactiveProperty();
        public QuestRecipe Recipe = new QuestRecipe();
        public FloatReactiveProperty Progress = new FloatReactiveProperty();
        public QuestTutorialExecutor TutorialExecutor;
        public QuestCallbacks Callbacks = new QuestCallbacks();

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            TutorialExecutor = GetComponentInChildren<QuestTutorialExecutor>();
        }

        #endregion
    }
}
