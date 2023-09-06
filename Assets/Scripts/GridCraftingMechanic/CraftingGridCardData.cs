using System;
using System.Collections.Generic;
using System.Linq;
using Cards.Core;
using Cards.Data;
using GridCraftingMechanic.Cards.GridCells;
using GridCraftingMechanic.Core;
using UniRx;
using UnityEngine;

namespace GridCraftingMechanic
{
    public class CraftingGridCardData : CardData
    {
        [Header("Grid Preferences")]
        public List<GridCellCardData> GridCells = new List<GridCellCardData>();
        public ReactiveProperty<GridRecipe> GridRecipe = new ReactiveProperty<GridRecipe>(new GridRecipe());

        public Action<CardData> OnSpawnedGridCard;

        #region MonoBehaviours

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
