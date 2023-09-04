using System.Collections.Generic;
using System.Linq;
using Cards.Core;
using Cards.Data;
using GridCraftingMechanic.Cards.GridCells;
using UniRx;
using UnityEngine;

namespace GridCraftingMechanic
{
    public class CraftingGridCardData : CardData
    {
        [Header("Grid Preferences")]
        public ReactiveProperty<Card> ResultedCard = new ReactiveProperty<Card>();
        public List<GridCellCardData> GridCells = new List<GridCellCardData>();

        #region MonoBehaviour

        public override void Validate()
        {
            base.Validate();

            Transform parent = transform.parent;

            if (parent == null) return;

            GridCells ??= parent.GetComponentsInChildren<GridCellCardData>(true).ToList();
        }

        #endregion
    }
}
