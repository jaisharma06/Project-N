using UnityEngine;

namespace  Characters.Common
{
    public abstract class HealthController : MonoBehaviour
    {
        public float maxHealth = 1f;
        public bool pCanTakeDamage { get; set; }

        protected float _currentHealth;

        private void Start()
        {
            _currentHealth = maxHealth;
        }
    
        public void TakeDamage(float damage)
        {
            if (!pCanTakeDamage)
            {
                return;   
            }
        
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        protected abstract void Die();
    }
}