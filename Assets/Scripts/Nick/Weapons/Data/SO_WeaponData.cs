using UnityEngine;

namespace ProjectN.Characters.Nick.Weapons.Data
{
    [CreateAssetMenu(fileName = "newWeaponData", menuName = "Data/Weapon Data/Weapon")]
    public class SO_WeaponData : ScriptableObject
    {
        public float[] movementSpeed;
    }
}
