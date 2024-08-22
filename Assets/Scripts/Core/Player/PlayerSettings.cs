using System;
using UnityEngine;

namespace Game.Core.Player
{
    [Serializable]
    public struct PlayerSettings
    {
        [field: SerializeField]
        public int Health
        {
            get;
            private set;
        }
        
        [field: SerializeField]
        public float MovementSpeed
        {
            get;
            private set;
        }
        
        [field: SerializeField]
        public float AttackDistance
        {
            get;
            private set;
        }

        [field: SerializeField]
        public float ReloadDuration
        {
            get;
            private set;
        }
        
        [field: SerializeField]
        public int Damage
        {
            get;
            private set;
        }

        [field: SerializeField]
        public float BulletSpeed
        {
            get;
            private set;
        }

        [field: SerializeField]
        public Rect MovementRect
        {
            get;
            private set;
        }
    }
}