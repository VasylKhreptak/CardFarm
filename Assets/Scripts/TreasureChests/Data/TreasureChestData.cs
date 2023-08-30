using TreasureChests.Graphics.Animations;
using TreasureChests.Logic.Tags;
using UnityEngine;
using Zenject;

namespace TreasureChests.Data
{
    public class TreasureChestData : MonoBehaviour, IValidatable
    {
        [SerializeField] private ChestHinge _chestHinge;

        [SerializeField] private ChestSpinAnimation _spinAnimation;
        [SerializeField] private ChestStateAnimation _stateAnimation;

        public ChestHinge ChestHinge => _chestHinge;

        public ChestSpinAnimation SpinAnimation => _spinAnimation;
        public ChestStateAnimation StateAnimation => _stateAnimation;

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
        }

        #endregion
    }
}
