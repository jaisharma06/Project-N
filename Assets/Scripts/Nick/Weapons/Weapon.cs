using ProjectN.Characters.Nick.States;
using ProjectN.Characters.Nick.Weapons.Data;
using UnityEngine;
using ProjectN.Characters.Enemy;

namespace ProjectN.Characters.Nick.Weapons
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private SO_WeaponData weaponData;

        protected Animator baseAnimator;
        protected Animator weaponAnimator;

        protected PlayerAttackState state;

        public int attackCounter;
        protected bool canDamageEnemy;

        protected virtual void Start()
        {
            baseAnimator = transform.Find("Base").GetComponent<Animator>();
            //weaponAnimator = transform.Find("Weapon").GetComponent<Animator>();

            gameObject.SetActive(false);
        }

        public virtual void EnterWeapon()
        {
            gameObject.SetActive(true);

            baseAnimator.SetBool("attack", true);
            weaponAnimator?.SetBool("attack", true);

            baseAnimator.SetInteger("attackCounter", attackCounter);
            weaponAnimator?.SetInteger("attackCounter", attackCounter);
            attackCounter++;
            if (attackCounter >= weaponData.movementSpeed.Length)
            {
                attackCounter = 0;
            }
        }

        public virtual void ExitWeapon()
        {
            baseAnimator.SetBool("attack", false);
            weaponAnimator?.SetBool("attack", false);

            //attackCounter++;

            gameObject.SetActive(false);
        }

        #region Animation Triggers

        public virtual void AnimationFinishTrigger()
        {
            state.AnimationFinishTrigger();
        }

        public virtual void AnimationStartMovementTrigger()
        {
            state.SetPlayerVelocity(weaponData.movementSpeed[attackCounter]);
        }

        public virtual void AnimationTurnOnEnemyDamage()
        {
            canDamageEnemy = true;
        }

        public virtual void AnimationTurnOffEnemyDamage()
        {
            canDamageEnemy = false;
        }

        public virtual void AnimationStopMovementTrigger()
        {
            state.SetPlayerVelocity(0f);
        }

        public virtual void AnimationTurnOffFlipTrigger()
        {
            state.SetFlipCheck(false);
        }

        public virtual void AnimationTurnOnFlipTigger()
        {
            state.SetFlipCheck(true);
        }

        #endregion

        public void InitializeWeapon(PlayerAttackState state)
        {
            this.state = state;
        }

        public virtual void ApplyDamageToEnemy(EnemyController enemy)
        {
            if (!canDamageEnemy || !enemy)
                return;
            enemy.health.TakeDamage(weaponData.damage);
        }

        #region CollisionTriggers
        private void OnTriggerStay2D(Collider2D other)
        {
            Debug.Log(canDamageEnemy);
            if (!canDamageEnemy)
            {
                return;
            }

            var enemy = other.GetComponent<EnemyController>();
            ApplyDamageToEnemy(enemy);
        }
        #endregion

    }
}
