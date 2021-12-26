using ProjectN.Characters.Nick.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectN.Characters.Nick.Intermediaries
{
    public class WeaponAnimationToWeapon : MonoBehaviour
    {
        private Weapon weapon;

        private void Start()
        {
            weapon = GetComponentInParent<Weapon>();
        }

        private void AnimationFinishTrigger()
        {
            weapon.AnimationFinishTrigger();
        }

        private void AnimationStartMovementTrigger()
        {
            weapon.AnimationStartMovementTrigger();
        }

        private void AnimationStopMovementTrigger()
        {
            weapon.AnimationStopMovementTrigger();
        }

        private void AnimationTurnOffFlipTrigger()
        {
            weapon.AnimationTurnOffFlipTrigger();
        }

        private void AnimationTurnOnFlipTrigger()
        {
            weapon.AnimationTurnOnFlipTigger();
        }

        private void AnimationTurnOnEnemyDamage()
        {
            weapon.AnimationTurnOnEnemyDamage();
        }

        private void AnimationTurnOffEnemyDamage()
        {
            weapon.AnimationTurnOffEnemyDamage();
        }

    }
}
