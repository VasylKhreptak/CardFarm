using System;
using ScriptableObjects.Scripts.Cards.AutomatedFactories.Recipes;
using UnityEngine;

namespace ScriptableObjects.Scripts.Cards.Incubators.Recipes
{
    [Serializable]
    public class IncubatorRecipe : FactoryRecipe
    {
        [Header("Preferences")]
        [SerializeField] private bool _removeResourcesOnComplete = true;
        [SerializeField] private bool _linkResultToIncubator;

        public bool RemoveResourcesOnComplete => _removeResourcesOnComplete;
        public bool LinkResultToIncubator => _linkResultToIncubator;
    }
}
