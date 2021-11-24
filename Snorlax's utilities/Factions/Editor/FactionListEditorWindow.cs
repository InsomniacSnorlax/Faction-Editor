using UnityEngine;
using UnityEditor;

namespace Snorlax.Factions
{
    public class FactionListEditorWindow : ExtendedEditorWindow
    {
        protected string selectedPropertyPach;
        protected string selectedProperty;

        Vector2 scrollPositionFaction = Vector2.zero;
        Vector2 scrollPositionRelation = Vector2.zero;

        private void OnGUI()
        {
            factions = GetAllInstances<Factions>();

            if (factions.Length > 0)
                serializedObject = new SerializedObject(factions[0]);

            foreach (Factions fact in factions)
            {
                fact.CheckRelationshipValues();
            }

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));
            scrollPositionFaction = EditorGUILayout.BeginScrollView(scrollPositionFaction);

            DrawSliderBar(factions);

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));

            //Displaying current selected faction from left panel to right panel

            if (selectedProperty != null)
            {
                for (int i = 0; i < factions.Length; i++)
                    if (factions[i].name == selectedProperty)
                    {
                        serializedObject = new SerializedObject(factions[i]);
                        serializedProperty = serializedObject.FindProperty("relations");
                        DrawProperties(serializedProperty, true, factions[i]);
                    }
            }
            else
            {
                EditorGUILayout.LabelField("select an item from the lsit");
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            if (factions.Length > 0)
                Apply();
        }

        /// <summary>
        /// Allows for the editor menu to appear on the tool bar
        /// </summary>

        [MenuItem("Tools/Factions")]
        protected static void ShowWindow()
        {
            GetWindow<FactionListEditorWindow>("Factions");
        }

        /// <summary>
        /// Displays list of all current factions on left panel
        /// </summary>
        /// <param name="prop"> is list of all factions found </param>

        protected void DrawSliderBar(Factions[] prop)
        {
            foreach (Factions p in prop)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button(p.factionName))
                {
                    selectedPropertyPach = p.name;
                }

                //Delte function

                if (GUILayout.Button("-", GUILayout.MaxWidth(20)))
                {
                    DeleteRelations(p);
                    selectedProperty = null;
                    AssetDatabase.DeleteAsset("Assets/Snorlax's utilities/Factions/ScriptableObject/" + p.name + ".asset"); //Deletes current faction
                    factions = GetAllInstances<Factions>();
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }

                EditorGUILayout.EndHorizontal();
            }

            if (!string.IsNullOrEmpty(selectedPropertyPach))
            {
                selectedProperty = selectedPropertyPach;
            }

            if (GUILayout.Button("New Faction"))
            {
                Factions newFaction = ScriptableObject.CreateInstance<Factions>();
                CreateFactionWindow newFactionWindow = GetWindow<CreateFactionWindow>("New Faction");
                newFactionWindow.newFaction = newFaction;
            }
        }

        /// <summary>
        /// Draws all the information of the selected scriptableObject and including
        /// scroll view for list of relations
        /// </summary>
        /// <param name="prop"> Selected property from faction button of left panel </param>
        /// <param name="drawChildren"> bool to include children in property field </param>
        /// <param name="faction"> Selected faction scriptableObject from faction button of left panel </param>

        protected void DrawProperties(SerializedProperty prop, bool drawChildren, Factions faction)
        {
            EditorGUILayout.LabelField(faction.factionName);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("factionIcon"), true);

            scrollPositionRelation = EditorGUILayout.BeginScrollView(scrollPositionRelation);

            foreach (SerializedProperty p in prop)
            {
                if (p.FindPropertyRelative("faction").objectReferenceValue != faction)
                {
                    EditorGUILayout.BeginVertical("box");
                    p.isExpanded = (EditorGUILayout.Foldout(p.isExpanded, p.FindPropertyRelative("faction").objectReferenceValue.name, EditorStyles.foldout));

                    if (p.isExpanded == true)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(p.FindPropertyRelative("faction"), drawChildren);
                        EditorGUILayout.PropertyField(p.FindPropertyRelative("currentRelationship"), drawChildren);
                        //EditorGUILayout.PropertyField(p.FindPropertyRelative("Icon"), drawChildren);
                        EditorGUILayout.PropertyField(p.FindPropertyRelative("relationValue"), drawChildren);
                        EditorGUI.indentLevel--;
                    }

                    EditorGUILayout.EndVertical();
                }
            }

            EditorGUILayout.EndScrollView();

            if (factions.Length > 1)
                SameValue(faction);
        }
    }
}