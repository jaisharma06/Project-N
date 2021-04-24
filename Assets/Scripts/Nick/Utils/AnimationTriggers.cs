using ProjectN.Characters.Nick.FiniteStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectN.Characters.Nick.Utils
{
    public class AnimationTriggers : MonoBehaviour
    {
        private Player _player;
        // Start is called before the first frame update
        void Awake()
        {
            _player = GetComponentInParent<Player>();
        }

        private void AnimationTrigger()
        {
            _player.AnimationTrigger();
        }

        private void AnimationFinishTrigger()
        {
            _player.AnimtionFinishTrigger();
        }
    }
}