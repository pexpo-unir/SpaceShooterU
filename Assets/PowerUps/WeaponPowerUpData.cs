using Ships;
using UnityEngine;
using Weapons;

namespace PowerUps
{
    [CreateAssetMenu(fileName = "WeaponPowerUpData", menuName = "PowerUps/WeaponPowerUpData")]
    public class WeaponPowerUpData : PowerUpData
    {
        [field: SerializeField]
        public WeaponData WeaponData { get; set; }

        public override void ApplyEffect(Ship ship)
        {
            ship.ChangeWeapon(WeaponData);
        }
    }
}
