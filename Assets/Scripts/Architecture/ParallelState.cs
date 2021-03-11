using System;

namespace Anvarat.Architecture
{
    public class ParallelState : State
    {
        private Action _context;
        public override Type Tick()
        {
            _context?.Invoke();
            return typeof(ParallelState);
        }

        public void SetContext(Action context)
        {
            _context = context;
        }
    }    
}

