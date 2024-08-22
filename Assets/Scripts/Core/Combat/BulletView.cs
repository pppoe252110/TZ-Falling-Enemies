using UnityEngine;

namespace Game.Core.Player
{
    public sealed class BulletView
        : MonoBehaviour
    {
        public void Move(Vector2 delta)
        {
            transform.Translate(delta, Space.World);
            transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg - 90f);
        }
    }
}