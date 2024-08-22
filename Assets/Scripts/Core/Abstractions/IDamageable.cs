using System;

namespace Game.Core.Abstractions
{
    public interface IDamageable
    {
        event Action OnDeath;
        
        int Health
        {
            get;
        }

        bool Hit(int amount);
    }
}