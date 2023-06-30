using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Extensions
{
    public static class ComponentLoader
    {
        public static List<T> LoadAllComponents<T>(string path)
        {
            List<T> components = new List<T>();

            string[] prefabPaths = AssetDatabase.FindAssets("t:prefab", new[] { path });

            foreach (string prefabGUID in prefabPaths)
            {
                string prefabPath = AssetDatabase.GUIDToAssetPath(prefabGUID);
                GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject));

                T[] prefabComponents = prefab.GetComponentsInChildren<T>(true);
                components.AddRange(prefabComponents);
            }

            return components;
        }
    }
}
