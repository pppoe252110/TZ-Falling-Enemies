using System;
using UnityEngine;

namespace Game.Utils
{
    [Serializable]
    public struct Range<T>
    {
        [field: SerializeField]
        public T Min
        {
            get; 
            private set;
        }

        [field: SerializeField]
        public T Max
        {
            get; 
            private set;
        }
    }
}
