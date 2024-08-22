using System;
using Game.Core.Abstractions;

namespace Game.Core.Player
{
    public sealed class PlayerModel: IDamageable
    {
        public PlayerModel(int health)
        {
            Health = health;
        }
        
        public event Action OnDeath = () => { };
        public event Action<int> OnHealthChanged = _ => { };
        
        public int Health
        {
            get; 
            private set;
        }

        public bool Hit(int amount)
        {
            Health -= amount;
            OnHealthChanged.Invoke(Health);
            
            return Health <= 0;
        }
    }
}