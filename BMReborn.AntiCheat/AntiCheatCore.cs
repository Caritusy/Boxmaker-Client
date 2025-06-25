using System;
using System.Collections.Generic;
using System.Text;

namespace BMReborn.AntiCheat
{
    internal class AntiCheatCore
    {

        public string Name { get; protected set; }

        public virtual bool IsGameCheated { get { return true; } }

        public virtual void Punish() { }

        public virtual void Update() { }

        public virtual void Reset() { }
    }
}
