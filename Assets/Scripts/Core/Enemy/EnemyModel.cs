using System;
using Game.Core.Abstractions;

namespace Game.Core.Enemy
{
    public sealed class EnemyModel
        : IDamageable
    {
        public EnemyModel(int health)
        {
            Health = health;
        }

        public event Action OnDeath = () => { };

        public int Health
        {
            get;
            private set;
        }
        
        public bool Hit(int amount)
        {
            Health -= amount;
            
            var isDead = Health <= 0;
            if (isDead)
            {
                OnDeath.Invoke();
            }
            
            return isDead;
        }
    }
}