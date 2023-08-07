using Quests.Data;
using UnityEngine;

namespace Quests.Logic.Core
{
    public class QuestReseter : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private QuestData _questData;

        #region MonoBehaviour

        private void OnValidate()
        {
            _questData ??= GetComponent<QuestData>();
        }

        private void OnDisable()
        {
            ResetData();
        }

        #endregion

        private void ResetData()
        {
            _questData.IsCompletedByAction.Value = false;
            _questData.IsCompleted.Value = false;
            _questData.TookReward.Value = false;
            _questData.Progress.Value = 0;
        }
    }
}
