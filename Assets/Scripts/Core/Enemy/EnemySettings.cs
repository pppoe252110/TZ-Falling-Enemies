using System;
using Game.Utils;
using UnityEngine;

namespace Game.Core.Enemy
{
    [Serializable]
    public struct EnemySettings
    {
        [field: SerializeField]
        public Range<float> SpawnDelayRange
        {
            get;
            private set;
        }

        [field: SerializeField]
        public Range<float> SpeedRange
        {
            get;
            private set;
        }

        [field: SerializeField]
        public int Health
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
        public float FinishLine
        {
            get;
            private set;
        }
    }
}