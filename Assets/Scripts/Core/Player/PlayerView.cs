using UnityEngine;

namespace Game.Core.Player
{
    [AddComponentMenu("Game/Views/Player")]
    public sealed class PlayerView: MonoBehaviour
    {
        public void Move(Vector2 delta, Rect rect)
        {
            transform.Translate(delta, Space.World);
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, rect.x, rect.x + rect.width), Mathf.Clamp(transform.position.y, rect.y, rect.y + rect.height));
        }
    }
}