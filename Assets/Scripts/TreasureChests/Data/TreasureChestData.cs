using TreasureChests.Graphics.Animations;
using TreasureChests.Logic;
using TreasureChests.Logic.Tags;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace TreasureChests.Data
{
    public class TreasureChestData : MonoBehaviour, IValidatable
    {
        [SerializeField] private ChestHinge _chestHinge;

        [SerializeField] private ChestSpinAnimation _spinAnimation;
        [SerializeField] private ChestStateAnimation _stateAnimation;
        [SerializeField] private ChestRewardOpener _chestRewardOpener;

        public ChestHinge ChestHinge => _chestHinge;

        public ChestSpinAnimation SpinAnimation => _spinAnimation;
        public ChestStateAnimation StateAnimation => _stateAnimation;
        public ChestRewardOpener ChestRewardOpener => _chestRewardOpener;

        public BoolReactiveProperty IsOpened = new BoolReactiveProperty(false);

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _chestHinge = GetComponentInChildren<ChestHinge>(true);

            _spinAnimation = GetComponentInChildren<ChestSpinAnimation>(true);
            _stateAnimation = GetComponentInChildren<ChestStateAnimation>(true);
            _chestRewardOpener = GetComponentInChildren<ChestRewardOpener>(true);
        }

        #endregion
    }
}
