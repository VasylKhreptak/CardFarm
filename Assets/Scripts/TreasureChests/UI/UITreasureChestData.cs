using System.Collections.Generic;
using Cards.Core;
using TreasureChests.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace TreasureChests.UI
{
    public class UITreasureChestData : MonoBehaviour, IValidatable
    {
        [Header("References")]
        [SerializeField] private UIBehaviour _raycastTarget;
        [SerializeField] private TreasureChestData _chestData;
        [SerializeField] private TreasureController _treasureController;

        [Header("Treasure Preferences")]
        [SerializeField] private List<Card> _possibleTreasureCard = new List<Card>();

        public UIBehaviour RaycastTarget => _raycastTarget;
        public TreasureChestData ChestData => _chestData;
        public TreasureController TreasureController => _treasureController;

        public List<Card> PossibleTreasureCard => _possibleTreasureCard;

        #region MonoBehaviour

        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            _treasureController = GetComponentInChildren<TreasureController>(true);
        }

        #endregion
    }
}
