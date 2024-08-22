using TMPro;
using UnityEngine;

namespace Game.Core.Player
{
    public sealed class PlayerHealthView: MonoBehaviour
    {
        [SerializeField] private string format;
        [SerializeField] private TMP_Text text;
        
        public void SetHealth(int health)
        {
            text.text = string.Format(format, health.ToString());
        }
    }
}