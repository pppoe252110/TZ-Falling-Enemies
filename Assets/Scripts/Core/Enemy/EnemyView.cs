using UnityEngine;

namespace Game.Core.Enemy
{
    [AddComponentMenu("Game/Views/Enemy")]
    public sealed class EnemyView
        : MonoBehaviour
    {
        public void Move(float distance)
        {
            transform.Translate(Vector2.down * distance, Space.World);
        }
    }
}
