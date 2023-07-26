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
    }
}
