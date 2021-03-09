using System;
using Anvarat.Architecture;

namespace Characters.Nick.States
{
    public class Jumping : State
    {
        private NickController _owner;

        public Jumping(NickController owner)
        {
            _owner = owner;
        }
        public override Type Tick()
        {
            if (!_owner.pGroundChecker.pIsGrounded)
            {
                return typeof(Jumping);
            }
            else
            {
                return typeof(Idle);
            }
        }
    }   
}