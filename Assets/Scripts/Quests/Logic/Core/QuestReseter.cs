using Quests.Data;
using Runtime.Commands;
using UnityEngine;
using Zenject;

namespace Quests.Logic.Core
{
    public class QuestReseter : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private QuestData _questData;

        private GameRestartCommand _gameRestartCommand;

        [Inject]
        private void Constructor(GameRestartCommand gameRestartCommand)
        {
            _gameRestartCommand = gameRestartCommand;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _questData ??= GetComponent<QuestData>();
        }

        private void Awake()
        {
            _gameRestartCommand.OnExecute += ResetData;
        }

        private void OnDestroy()
        {
            _gameRestartCommand.OnExecute -= ResetData;
        }

        #endregion

        private void ResetData()
        {
            _questData.IsCompletedByAction.Value = false;
            _questData.IsCompleted.Value = false;
            _questData.TookReward.Value = false;
            _questData.Progress.Value = 0;
            _questData.ResultedCard = null;
        }
    }
}
