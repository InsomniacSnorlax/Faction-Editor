using UnityEngine;
using UnityEditor;
using Snorlax.Settings;

namespace Snorlax.Factions
{
    public class CreateFactionWindow : EditorWindow
    {
        public Factions newFaction;
        public SerializedObject serializedObject;
        private Factions[] factions;
        
        private void OnInspectorUpdate()
        {
            Repaint();
        }

        /// <summary>
        /// Creates new faction scriptableobject based on input values
        /// </summary>

        private void OnGUI()
        {
            EditorGUI.BeginChangeCheck();
            {
                DrawProperties(serializedObject);
            }
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(newFaction);
            }

            if (GUILayout.Button("Save"))
            {
                SavePathEditor.SavePath(SavePathEditor.FactionKeyName, SavePathEditor.FactionSavePath, newFaction, newFaction.factionName);
                factions = FactionHelper.GetAllInstances<Factions>();
                FactionHelper.NewRelations(newFaction, factions);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                GetWindow<FactionListEditorWindow>().factions = factions;
                Close();
            }
        }

        public void Init(Factions newFaction)
        {
            this.newFaction = newFaction;
            serializedObject = new SerializedObject(newFaction);
        }

        protected void DrawProperties(SerializedObject p)
        {
            EditorGUILayout.PropertyField(p.FindProperty("factionName"), true);
            EditorGUILayout.PropertyField(p.FindProperty("factionIcon"), true);
        }
    }
}