using DamageSystem;
using Ships;
using UnityEngine;

namespace PowerUps
{
    [CreateAssetMenu(fileName = "HealingPowerUpData", menuName = "PowerUps/HealingPowerUpData")]
    public class HealingPowerUpData : PowerUpData
    {
        [field: SerializeField]
        public int HealingAmount { get; set; }

        public override void ApplyEffect(Ship ship)
        {
            if (ship.TryGetComponent<DamageComponent>(out var damageComponent))
            {
                damageComponent.GetHealing(HealingAmount);
            }
        }
    }
}
