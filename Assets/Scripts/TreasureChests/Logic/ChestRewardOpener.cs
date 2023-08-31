using System;
using Graphics.UI.Panels;
using TreasureChests.Data;
using UnityEngine;
using Zenject;

namespace TreasureChests.Logic
{
    public class ChestRewardOpener : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private TreasureChestData _chestData;

        private LevelUpPanel.LevelUpPanel _levelUpPanel;

        [Inject]
        private void Constructor(LevelUpPanel.LevelUpPanel levelUpPanel)
        {
            _levelUpPanel = levelUpPanel;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _chestData = GetComponentInParent<TreasureChestData>(true);
        }

        #endregion

        public void Open(Action onOpened = null)
        {
            _chestData.StateAnimation.Open(onOpened);
        }

        public void ShowReward()
        {
            
        }

        public void CollectReward()
        {

        }
    }
}
