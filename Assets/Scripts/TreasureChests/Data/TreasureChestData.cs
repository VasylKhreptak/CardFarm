using CBA.Animations.Core;
using TreasureChests.Graphics.Animations;
using TreasureChests.Logic;
using TreasureChests.Logic.Tags;
using UniRx;
using UnityEngine;
using Zenject;

namespace TreasureChests.Data
{
    public class TreasureChestData : MonoBehaviour, IValidatable
    {
        [SerializeField] private ChestHinge _chestHinge;

        [SerializeField] private ChestRewardOpener _chestRewardOpener;

        [Header("Animations")]
        [SerializeField] private ChestSpinAnimation _spinAnimation;
        [SerializeField] private ChestStateAnimation _stateAnimation;
        [SerializeField] private AnimationCore _scalePressAnimation;
        [SerializeField] private AnimationCore _scaleReleaseAnimation;

        public ChestHinge ChestHinge => _chestHinge;

        public ChestSpinAnimation SpinAnimation => _spinAnimation;
        public ChestStateAnimation StateAnimation => _stateAnimation;
        public ChestRewardOpener ChestRewardOpener => _chestRewardOpener;
        public AnimationCore ScalePressAnimation => _scalePressAnimation;
        public AnimationCore ScaleReleaseAnimation => _scaleReleaseAnimation;

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
