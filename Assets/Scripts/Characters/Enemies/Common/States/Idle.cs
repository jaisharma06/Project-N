using System;

namespace  Characters.Enemies.Common.State
{
    public class Idle : Anvarat.Architecture.State
    {
        private EnemyController _owner;
        public Idle(EnemyController owner)
        {
            _owner = owner;
        }
        
        public override Type Tick()
        {
            return typeof(Idle);
        }
    }   
}
