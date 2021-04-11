using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Nick.Settings
{
    [CreateAssetMenu(fileName = "NickSettings", menuName = "Characters/Settings/Nick")]
    public class Traits : ScriptableObject
    {
        public float maxSlope = 45f;
        public float jumpForce = 4f;
        public float fallMultiplier = 2.5f;
        public float lowJumpMultiplier = 2f;
        public float walkSpeed = 4f;
        public float dodgeSpeed = 10f;
        public float dodgeTime = 1f;
        public float dodgeCooldownTime = 1f;
        public float maxHealth = 1f;
        public string characterName = "Nick";
    }   
}
