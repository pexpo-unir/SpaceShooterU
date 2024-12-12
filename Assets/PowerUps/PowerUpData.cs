using Ships;
using UnityEngine;

namespace PowerUps
{
    [CreateAssetMenu(fileName = "PowerUpData", menuName = "PowerUps/PowerUpData")]
    public abstract class PowerUpData : ScriptableObject
    {
        [field: SerializeField]
        public Sprite Sprite { get; set; }

        [field: SerializeField]
        public float Speed { get; set; }

        public abstract void ApplyEffect(Ship ship);
    }
}
