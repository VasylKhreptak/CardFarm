using System;
using System.Collections.Generic;
using ScriptableObjects.Scripts.Cards.Recipes;
using UnityEngine;

namespace ScriptableObjects.Scripts.Cards.Cattle
{
    [Serializable]
    public class CattleItemSpawnerRecipe
    {
        public float Cooldown = 90f;
        public List<CattleCardWeight> Weights = new List<CattleCardWeight>();
    }

    [Serializable]
    public class CattleCardWeight : CardWeight
    {
        [Range(0f, 1f)] public float SpawnChance = 1f;
    }
}
