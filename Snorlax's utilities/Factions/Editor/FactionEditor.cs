using UnityEngine;
using UnityEditor;

namespace Snorlax.Factions
{
    [CustomEditor(typeof(Factions))]
    public class FactionEditor : Editor
    {
        protected SerializedProperty Relations;

        private void OnEnable()
        {
            Relations = serializedObject.FindProperty("factionName");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(Relations);

            if (GUILayout.Button("Change Asset Name"))
            {
                string assetPath = AssetDatabase.GetAssetPath(serializedObject.targetObject);
                AssetDatabase.RenameAsset(assetPath, Relations.stringValue);
                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
                serializedObject.ApplyModifiedProperties();
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("relations"));
        }
    }
}