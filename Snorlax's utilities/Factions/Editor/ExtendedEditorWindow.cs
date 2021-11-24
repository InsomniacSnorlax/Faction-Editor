using System.Collections.Generic;
using UnityEditor;

namespace Snorlax.Factions
{
    public class ExtendedEditorWindow : EditorWindow
    {
        protected SerializedObject serializedObject;
        protected SerializedProperty serializedProperty;
        protected Factions[] factions;

        protected void Apply()
        {
            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Gets all instances of specific type and returns it as a list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns> returns Factions as a list </returns>
        
        protected static T[] GetAllInstances<T>() where T : Factions
        {
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
            T[] a = new T[guids.Length];

            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }

            return a;
        }

        /// <summary>
        /// New faction gets list of all current factions 
        /// Each faction gets a new relation with the new faction
        /// </summary>

        protected void NewRelations(Factions newFaction)
        {
            if (factions.Length > 0)
            {
                Relations[] temp = new Relations[factions.Length];
                for (int i = 0; i < factions.Length; i++)
                {
                    temp[i].faction = factions[i];
                }
                newFaction.relations = temp;
            }

            foreach (Factions fact in factions)
            {
                if (fact.relations.Length != factions.Length)
                {
                    Relations it = new Relations();
                    it.faction = newFaction;
                    fact.relations = AddItemToArray(fact.relations, it);
                }
            }
        }

        /// <summary>
        /// Removes all relations that include the specified faction
        /// </summary>

        protected void DeleteRelations(Factions newFaction)
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

        /// <summary>
        /// Extended of NewRelations() that returns relation array with included item
        /// </summary>

        protected static Relations[] AddItemToArray(Relations[] original, Relations itemToAdd)
        {
            Relations[] finalArray = new Relations[original.Length + 1];

            for (int i = 0; i < original.Length; i++)
            {
                finalArray[i] = original[i];
            }

            finalArray[finalArray.Length - 1] = itemToAdd;
            return finalArray;
        }

        /// <summary>
        /// Matches faction relationship value to the targeted faction in relations
        /// </summary>

        protected void SameValue(Factions fact)
        {
            for (int x = 0; x < factions.Length; x++)
            {
                for (int i = 0; i < factions[x].relations.Length; i++)
                {
                    if (fact == factions[x].relations[i].faction)
                    {
                        this.factions[x].relations[i].relationValue = SetValue(fact, factions[x]);
                    }
                }
            }
        }

        /// <summary>
        /// Finds the faction relationship value
        /// </summary>

        protected int SetValue(Factions fact, Factions f)
        {
            for (int i = 0; i < fact.relations.Length; i++)
            {
                if (fact.relations[i].faction == f)
                {
                    return fact.relations[i].relationValue;
                }
            }

            return 100;
        }
    }
}