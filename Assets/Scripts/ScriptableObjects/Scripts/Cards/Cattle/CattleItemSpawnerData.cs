using UnityEngine;

namespace ScriptableObjects.Scripts.Cards.Cattle
{

    [CreateAssetMenu(fileName = "CattleItemSpawnerData", menuName = "ScriptableObjects/CattleItemSpawnerData")]
    public class CattleItemSpawnerData : ScriptableObject
    {
        public CattleItemSpawnerRecipe Recipe = new CattleItemSpawnerRecipe();
    }
}
