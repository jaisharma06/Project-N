using System;

namespace ProjectN.Characters
{
    public interface IHealth : IDisposable
    {
        bool isDead { get; set; }
        void TakeDamage(float damage);
        void Die();
    }
}