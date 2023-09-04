using System;
using Cards.Data;
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
        public BoolReactiveProperty IsCompletedByAction = new BoolReactiveProperty();
        public BoolReactiveProperty TookReward = new BoolReactiveProperty();
        public QuestRecipe Recipe = new QuestRecipe();
        public BoolReactiveProperty IsCompleted = new BoolReactiveProperty();
        public QuestTutorial Tutorial;
        public QuestCallbacks Callbacks = new QuestCallbacks();

        public CardData ResultedCard;

        public bool LevelUpAsReward = false;

        public BoolReactiveProperty IsCurrentQuest = new BoolReactiveProperty();

        [Header("Progress Preferences")]
        public FloatReactiveProperty Progress = new FloatReactiveProperty();
        public IntReactiveProperty CurrentQuantity = new IntReactiveProperty();
        public IntReactiveProperty TargetQuantity = new IntReactiveProperty();

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            Tutorial = GetComponentInChildren<QuestTutorial>(true);
        }

        #endregion
    }
}
