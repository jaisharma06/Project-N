using UnityEngine;
namespace ProjectN.Characters.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        public Health health;

        private void Awake()
        {
            health = GetComponent<Health>();
        }

        private void OnEnable()
        {
            health.OnTakeDamage += OnTakeDamage;
            health.OnDie += OnDie;
        }
        private void OnDisable()
        {
            health.OnTakeDamage -= OnTakeDamage;
            health.OnDie -= OnDie;
        }

        private void OnTakeDamage(float damage)
        {
            Debug.Log("Enemy took damage: " + name);
        }

        private void OnDie()
        {
            Debug.Log("Enemy died: " + name);
            Destroy(gameObject, 0);
        }
    }
}
