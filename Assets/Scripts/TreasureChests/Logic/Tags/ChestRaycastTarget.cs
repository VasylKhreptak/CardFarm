using TreasureChests.Data;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TreasureChests.Logic.Tags
{
    public class ChestRaycastTarget : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private UIBehaviour _uiBehaviour;
        [SerializeField] private TreasureChestData _chestData;

        public UIBehaviour UIBehaviour => _uiBehaviour;
        public TreasureChestData ChestData => _chestData;

        #region MonoBehaviour

        private void OnValidate()
        {
            _uiBehaviour = GetComponent<UIBehaviour>();
        }

        #endregion
    }
}
