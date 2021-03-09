using System;

namespace Anvarat.Architecture
{
    public abstract class State
    {
        public abstract Type Tick();
        public virtual void OnEnter() { }
        public virtual void OnExit() { }
    }
}
