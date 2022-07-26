using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Snorlax.Factions
{
    public class FactionListEditorWindow : EditorWindow
    {
        private int SelectedFaction = -1;

        Vector2 scrollPositionFaction = Vector2.zero;
        Vector2 scrollPositionRelation = Vector2.zero;

        protected SerializedObject serializedObject;
        protected SerializedProperty serializedProperty;
        public Factions[] factions;

        /// <summary>
        /// Allows for the editor menu to appear on the tool bar
        /// </summary>

        [MenuItem("Snorlax's Tools/Factions")]
        protected static void ShowWindow()
        {
            GetWindow<FactionListEditorWindow>("Factions");
        }

        private void OnEnable()
        {
            factions = FactionHelper.GetAllInstances<Factions>();
        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));
                {
                    scrollPositionFaction = EditorGUILayout.BeginScrollView(scrollPositionFaction);
                    {
                        DrawFactionList();
                    }
                    EditorGUILayout.EndScrollView();
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
                {
                    //Displaying current selected faction from left panel to right panel

                    if (SelectedFaction != -1 )
                    {
                        EditorGUI.BeginChangeCheck();
                        {
                            serializedObject = new SerializedObject(factions[SelectedFaction]);
                            serializedProperty = serializedObject.FindProperty("relations");
                            DrawProperties();
                        }
                        if (EditorGUI.EndChangeCheck())
                        {
                            foreach (Factions fact in factions)
                            {
                                fact.CheckRelationshipValues();
                            }

                            SameValue(factions[SelectedFaction]);
                            serializedObject.ApplyModifiedProperties();
                            EditorUtility.SetDirty(factions[SelectedFaction]);
                            //AssetDatabase.Refresh();
                        }
                    }
                    else
                    {
                        EditorGUILayout.LabelField("select an item from the lsit");
                    }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Displays list of all current factions on left panel
        /// </summary>
        /// <param name="prop"> is list of all factions found </param>

        private void DrawFactionList()
        {
            for(int i = 0; i < factions.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button(factions[i].factionName))
                    {
                        SelectedFaction = i;
                        GUI.FocusControl(null);
                    }

                    if (GUILayout.Button("-", GUILayout.MaxWidth(20)))
                    {
                        SelectedFaction = -1;
                        DeleteRelations(factions[i]);
                        string assetPath = AssetDatabase.GetAssetPath(new SerializedObject(factions[i]).targetObject);
                        AssetDatabase.DeleteAsset(assetPath);
                        factions = FactionHelper.GetAllInstances<Factions>();
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }
                }
                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("New Faction"))
            {
                Factions newFaction = ScriptableObject.CreateInstance<Factions>();
                CreateFactionWindow newFactionWindow = GetWindow<CreateFactionWindow>("New Faction");
                newFactionWindow.Init(newFaction);
            }
        }

        /// <summary>
        /// Draws all the information of the selected scriptableObject and including
        /// scroll view for list of relations
        /// </summary>
        /// <param name="prop"> Selected property from faction button of left panel </param>
        /// <param name="drawChildren"> bool to include children in property field </param>
        /// <param name="faction"> Selected faction scriptableObject from faction button of left panel </param>

        private void DrawProperties()
        {
            EditorGUILayout.LabelField(factions[SelectedFaction].factionName);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("factionIcon"), true);

            scrollPositionRelation = EditorGUILayout.BeginScrollView(scrollPositionRelation);
            {
                foreach (SerializedProperty p in serializedProperty)
                {
                    if (p.FindPropertyRelative("faction").objectReferenceValue == factions[SelectedFaction])
                    {
                        continue;
                    }

                    EditorGUILayout.BeginVertical("box");
                    {
                        p.isExpanded = (EditorGUILayout.Foldout(p.isExpanded, p.FindPropertyRelative("faction").objectReferenceValue.name, EditorStyles.foldout));

                        if (p.isExpanded == true)
                        {
                            EditorGUI.indentLevel++;
                            EditorGUILayout.PropertyField(p.FindPropertyRelative("faction"), true);
                            EditorGUILayout.PropertyField(p.FindPropertyRelative("currentRelationship"), true);
                            //EditorGUILayout.PropertyField(p.FindPropertyRelative("Icon"), drawChildren);
                            EditorGUILayout.PropertyField(p.FindPropertyRelative("relationValue"), true);
                            EditorGUI.indentLevel--;
                        }
                    }
                    EditorGUILayout.EndVertical();
                    
                }
            }
            EditorGUILayout.EndScrollView();
        }

        /// <summary>
        /// Matches faction relationship value to the targeted faction in relations
        /// </summary>
        private void SameValue(Factions fact)
        {
            for (int x = 0; x < factions.Length; x++)
            {
                for (int i = 0; i < factions[x].relations.Length; i++)
                {
                    if (fact == factions[x].relations[i].faction)
                    {
                        this.factions[x].relations[i].relationValue = FactionHelper.SetValue(fact, factions[x]);
                    }
                }
            }
        }

        /// <summary>
        /// Removes all relations that include the specified faction
        /// </summary>
        private void DeleteRelations(Factions newFaction)
        {
            foreach (Factions fact in factions)
            {
                List<Relations> newRelation = new List<Relations>();

                foreach (Relations real in fact.relations)
                {
                    if (real.faction != newFaction)
                    {
                        newRelation.Add(real);
                    }
                }

                fact.relations = newRelation.ToArray();
                newRelation.Clear();
            }
        }
    }
}