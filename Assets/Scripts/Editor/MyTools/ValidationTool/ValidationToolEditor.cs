using System.Collections.Generic;
using Editor.Extensions;
using Extensions;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace Editor.MyTools.ValidationTool
{
    public class ValidationToolEditor : EditorWindow
    {
        private string _prefabPath = "Assets/Prefabs";

        [MenuItem("MyTools/Validation")]
        public static void ShowWindow()
        {
            GetWindow<ValidationToolEditor>("Validation Tool");
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Validate Scripts"))
            {
                ValidatePrefabs();
            }

            _prefabPath = GUILayout.TextArea(_prefabPath);

            GUILayout.EndHorizontal();
        }

        private void ValidatePrefabs()
        {
            List<IValidatable> validators = ComponentLoader.LoadAllComponents<IValidatable>(_prefabPath);

            validators.ForEach(validator => validator.Validate());

            Debug.Log($"Validated {validators.Count} scripts in {_prefabPath}.");
        }
    }
}
