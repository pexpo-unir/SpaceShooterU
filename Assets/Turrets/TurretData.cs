using Lasers;
using UnityEngine;
using Weapons;

namespace Turrets
{
    [CreateAssetMenu(fileName = "TurretData", menuName = "Turrets/TurretData")]
    public class TurretData : ScriptableObject
    {
        [field: SerializeField]
        public string Tag { get; set; }

        [field: SerializeField]
        public Sprite Sprite { get; set; }

        [field: SerializeField]
        public Vector2 FacingDirection { get; set; }

        [field: SerializeField]
        public int MaxHealth { get; set; } = 1;

        [field: SerializeField]
        public WeaponData WeaponData { get; set; }

        [field: SerializeField]
        public LaserData LaserData { get; set; }

        [field: SerializeField]
        public float ShootRate { get; set; } = 0.5f;
    }
}
