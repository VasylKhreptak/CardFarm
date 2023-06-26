using UnityEngine;

namespace ScriptableObjects.Scripts.Cards.ResourceNodes
{
    [CreateAssetMenu(fileName = "ResourceNodeData", menuName = "ScriptableObjects/ResourceNodeData")]
    public class ResourceNodeData : ScriptableObject
    {
        public ResourceNodeRecipe Recipe;
    }
}
