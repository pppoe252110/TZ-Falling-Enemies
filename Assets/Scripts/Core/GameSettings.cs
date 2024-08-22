using Game.Core.Enemy;
using Game.Core.Player;
using Game.Utils;
using UnityEngine;

namespace Game.Core
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Game/Core/Game Settings")]
    public sealed class GameSettings : ScriptableObject
    {
        [field: SerializeField]
        public Range<int> KillsToWin
        {
            get;
            private set;
        }

        [field: SerializeField]
        public EnemySettings EnemySettings
        {
            get;
            private set;
        }

        [field: SerializeField]
        public PlayerSettings PlayerSettings
        {
            get;
            private set;
        }
    }
}
