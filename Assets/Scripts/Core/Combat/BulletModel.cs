using System;

namespace Game.Core.Player
{
    public sealed class BulletModel
    {
        public BulletModel(int damage, float speed)
        {
            Damage = damage;
            Speed = speed;
        }

        public int Damage
        {
            get;
        }

        public float Speed
        {
            get;
        }
    }
}