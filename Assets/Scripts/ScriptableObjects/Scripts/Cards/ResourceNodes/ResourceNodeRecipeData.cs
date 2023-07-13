using UnityEngine;

namespace ScriptableObjects.Scripts.Cards.ResourceNodes
{
    [CreateAssetMenu(fileName = "ResourceNodeRecipeData", menuName = "ScriptableObjects/ResourceNodeRecipeData")]
    public class ResourceNodeRecipeData : ScriptableObject
    {
        public ResourceNodeRecipe Recipe;
    }
}
