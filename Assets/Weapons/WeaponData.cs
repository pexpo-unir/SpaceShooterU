using UnityEngine;

namespace Weapons
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Weapons/WeaponData")]
    public class WeaponData : ScriptableObject
    {
        [field: SerializeField]
        public WeaponBase weaponBase { get; set; }

        [field: SerializeField]
        public float FireRate { get; set; }

        [field: SerializeField]
        public int ShotsNumber { get; set; } = 1;
    }
}
