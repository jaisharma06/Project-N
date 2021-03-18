using Characters.Common.Enums;
using UnityEngine;

namespace Characters.Common.Enums
{
    public enum EnemyType
    {
        FLYING,
        RANGED,
        MEELE
    }

    public enum EnemyState
    {
        PATROLLING,
        ALERT,
        SEARCHING,
        FIGHTING,
        IDLE,
        INACTIVE
    }
}

namespace Characters.Enemies.Common
{
    [CreateAssetMenu(fileName = "NickSettings", menuName = "Characters/Settings/Enemy")]
    public class EnemyTraits : ScriptableObject
    {
        public float maxHealth = 1f;
        public float damage = 0.2f;
        public float patrolSpeed = 4f;
        public float followSpeed = 4f;
        public EnemyState state;
        public EnemyType type;
        public string enemyName;
    } 
}