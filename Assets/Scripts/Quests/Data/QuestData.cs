using System;
using Quests.Logic.Core;
using UniRx;
using UnityEngine;

namespace Quests.Data
{
    [Serializable]
    public class QuestData : MonoBehaviour
    {
        public Quest Quest;
        public string Name;
        public QuestReward Reward;
        public BoolReactiveProperty IsCompleted = new BoolReactiveProperty();
        public BoolReactiveProperty TookReward = new BoolReactiveProperty();
    }
}
