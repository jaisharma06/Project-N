using UnityEngine;
using System;

namespace ProjectN.Characters
{
    public class Health : MonoBehaviour, IDisposable
    {
        public float maxHealth; //maximum health of the player.
        public float damageCooldownTime = 0.3f; //time in seconds between taking damage.
        private float currentHealth { get; set; }
        private float damageCooldownTimer { get; set; }
        private bool canTakeDamage { get; set; }
        public bool isDead { get; set; }
        public event Action<float> OnTakeDamage;
        public event Action OnDie;

        private void Awake()
        {
            currentHealth = maxHealth;
            canTakeDamage = true;
            isDead = false;
        }

        private void Update()
        {
            UpdateDamageCooldownTimer();
        }

        public void TakeDamage(float damage)
        {
            if (isDead || !canTakeDamage)
                return;
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                canTakeDamage = false;
                damageCooldownTimer = damageCooldownTime;
                OnTakeDamage?.Invoke(damage);
            }
        }

        public void Die()
        {
            if (isDead)
                return;

            isDead = true;
            OnDie?.Invoke();
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
            OnTakeDamage = null;
            OnDie = null;
        }
    }
}