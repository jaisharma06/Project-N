using UnityEngine;

namespace Characters.Nick
{
    public class AttackHandler : MonoBehaviour
    {
        public Transform m_attackPoint;
        public float m_range;
        public float m_damage;
        public LayerMask m_enemyLayer;

        public void DamageEnemy()
        {
            var enemy = Physics2D.OverlapCircle(m_attackPoint.position, m_range, m_enemyLayer);
#if UNITY_EDITOR
            Debug.Log(enemy.name);
#endif
        }

        private void OnDrawGizmosSelected()
        {
            if (!m_attackPoint)
                return;
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(m_attackPoint.position, m_range);
        }
    }
}