using UnityEngine;
using UnityEditor;

namespace Snorlax.Factions
{
    public class CreateFactionWindow : ExtendedEditorWindow
    {
        public Factions newFaction;

        /// <summary>
        /// Creates new faction scriptableobject based on input values
        /// </summary>

        private void OnGUI()
        {
            serializedObject = new SerializedObject(newFaction);
            DrawProperties(serializedObject);

            if (GUILayout.Button("Save"))
            {
                AssetDatabase.CreateAsset(newFaction, "Assets/Snorlax's utilities/Factions/ScriptableObject/" + newFaction.factionName + ".asset");
                factions = GetAllInstances<Factions>();
                NewRelations(newFaction);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Close();
            }

            Apply();
        }

        protected void DrawProperties(SerializedObject p)
        {
            EditorGUILayout.PropertyField(p.FindProperty("factionName"), true);
            EditorGUILayout.PropertyField(p.FindProperty("factionIcon"), true);
        }
    }
}