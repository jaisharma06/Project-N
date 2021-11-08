using System;

namespace ProjectN.Characters
{
    public interface IHealth : IDisposable
    {
        float maxHealth { get; set; }
        bool isDead { get;set; }
        void TakeDamage(float damage);
        void Die();
    }
}