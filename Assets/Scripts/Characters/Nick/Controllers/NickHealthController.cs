using Characters.Common;

namespace Characters.Nick
{
    public class NickHealthController : HealthController
    {
        public void SetMaxHealth(float health)
        {
            maxHealth = health;
            _currentHealth = maxHealth;
        }
        
        protected override void Die()
        {
            
        }
    }
}
