using System.Collections.Generic;
using EditorTools.Validators;
using Extensions;
using UnityEditor;
using UnityEngine;

namespace Editor.MyTools
{
    public class MyToolsEditor : EditorWindow
    {
        private string _prefabPath = "Assets/Prefabs";

        [MenuItem("MyTools/Tools")]
        public static void ShowWindow()
        {
            GetWindow<MyToolsEditor>("My Tools");
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Validate Prefabs"))
            {
                ValidatePrefabs();
            }

            _prefabPath = GUILayout.TextArea(_prefabPath);

            GUILayout.EndHorizontal();
        }

        private void ValidatePrefabs()
        {
            List<ChildrenValidator> validators = ComponentLoader.LoadAllComponents<ChildrenValidator>(_prefabPath);

            validators.ForEach(validator => validator.OnValidate());
        }
    }
}
