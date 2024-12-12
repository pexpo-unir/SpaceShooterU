using Lasers;
using UnityEngine;
using Weapons;

namespace Ships
{
    [CreateAssetMenu(fileName = "ShipData", menuName = "Ships/ShipData")]
    public class ShipData : ScriptableObject
    {
        [field: SerializeField]
        public string Tag { get; set; }

        [field: SerializeField]
        public Sprite Sprite { get; set; }

        [field: SerializeField]
        public Vector2 FacingDirection { get; set; }

        [field: SerializeField]
        public float Speed { get; set; }

        [field: SerializeField]
        public int MaxHealth { get; set; } = 1;

        [field: SerializeField]
        public WeaponData WeaponData { get; set; }

        [field: SerializeField]
        public LaserData LaserData { get; set; }
    }
}
