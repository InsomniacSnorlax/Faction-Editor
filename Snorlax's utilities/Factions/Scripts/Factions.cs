using UnityEngine;

namespace Snorlax.Factions
{
    [CreateAssetMenu(menuName = "Factions")]
    public class Factions : ScriptableObject
    {
        public string factionName;
        public Sprite factionIcon;
        public Relations[] relations;

        /// <summary>
        /// Sets currentRelationship to equal valueRelationship 
        /// in order for the for the value to appear in editor
        /// </summary>
    
        public void CheckRelationshipValues()
        {
            for (int i = 0; i < relations.Length; i++)
            {
                relations[i].currentRelationship = relations[i].valueRelationship;
                this.name = factionName;
            }
        }
    }

    /// <summary>
    /// Relations serializable for relationships between each faction
    /// <param name="valueRelationship"> Sets Enum based on relationValue</param>
    /// <typeparam name="Relations"> Relations is the current relation to the targeted faction </typeparam>
    /// </summary>

    [System.Serializable]

    public struct Relations
    {
        public Factions faction;
        public RelationStat currentRelationship;

        [Range(-100, 100)]
        public int relationValue;

        public RelationStat valueRelationship =>
            relationValue < -75 ? RelationStat.Enemy
            : relationValue > -75 && relationValue < -25 ? RelationStat.Wary
            : relationValue > -25 && relationValue < 25 ? RelationStat.Neutral
            : relationValue > 25 && relationValue < 75 ? RelationStat.Indifferent
            : relationValue > 75 ? RelationStat.Ally
            : RelationStat.Neutral;
    }

    public enum RelationStat
    {
        Ally,
        Indifferent,
        Neutral,
        Wary,
        Enemy
    }
}