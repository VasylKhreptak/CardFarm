using System.Collections.Generic;
using UnityEngine;

namespace Extensions
{
    public static class GameObjectExtensions
    {
        public static List<GameObject> GetChildren(this GameObject gameObject)
        {
            var children = new List<GameObject>();

            foreach (Transform child in gameObject.transform)
            {
                children.Add(child.gameObject);
            }

            return children;
        }

        public static void SetLayerRecursive(this GameObject gameObject, int _layer)
        {
            gameObject.layer = _layer;
            
            foreach (Transform child in gameObject.transform)
            {
                child.gameObject.layer = _layer;

                Transform _HasChildren = child.GetComponentInChildren<Transform>();

                if (_HasChildren != null)
                    SetLayerRecursive(child.gameObject, _layer);
            }
        }
    }
}
