using System.Collections.Generic;
using UnityEditor;

namespace Snorlax.Factions
{
    public static class FactionHelper
    {
        /// <summary>
        /// Gets all instances of specific type and returns it as a list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns> returns Factions as a list </returns>

        public static T[] GetAllInstances<T>() where T : Factions
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

        public static void NewRelations(Factions newFaction, Factions[] factions)
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
        /// Extended of NewRelations() that returns relation array with included item
        /// </summary>

        public static Relations[] AddItemToArray(Relations[] original, Relations itemToAdd)
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
        /// Finds the faction relationship value
        /// </summary>

        public static int SetValue(Factions fact, Factions f)
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