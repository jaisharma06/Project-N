using UnityEngine;
namespace ProjectN.Characters.Enemy
{
    public class EnemyController : MonoBehaviour, IHealth
    {
        #region TRAITS
        public float damageCooldownTime = 1f;
        public float maxHealth = 2f;
        #endregion

        #region PRIVATE_MEMBERS
        [SerializeField]
        private bool canTakeDamage;
        private float damageCooldownTimer = 0;
        #endregion

        #region UNITY_BUILTINS
        private void Awake()
        {
            InitHealth();
        }

        private void Update()
        {
            UpdateDamageCooldownTimer();
        }
        #endregion

        #region HEALTH
        public bool isDead { get; set; }
        private float health;

        private void InitHealth()
        {
            health = maxHealth;
            canTakeDamage = true;
            isDead = false;
        }

        public void TakeDamage(float damage)
        {
            Debug.Log($"Taking Damage {isDead} {canTakeDamage}");
            if (isDead || !canTakeDamage)
            {
                return;
            }
            if (health > 0)
            {
                health -= damage;
                damageCooldownTimer = damageCooldownTime;
                canTakeDamage = false;
                Debug.Log("Damage applied to: " + name);
            }

            if (health <= 0)
            {
                health = 0;
                Die();
            }
        }

        public void Die()
        {
            isDead = true;
            Destroy(gameObject, 0);
        }

        private void UpdateDamageCooldownTimer()
        {
            if (damageCooldownTimer <= 0)
            {
                return;
            }

            damageCooldownTimer -= Time.deltaTime;

            if (damageCooldownTimer <= 0)
            {
                damageCooldownTimer = 0;
                canTakeDamage = true;
            }
        }

        public void Dispose()
        {

        }
        #endregion
    }
}
