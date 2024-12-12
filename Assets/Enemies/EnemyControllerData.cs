using Ships;
using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "EnemyControllerData", menuName = "Enemies/EnemyControllerData")]
    public class EnemyControllerData : ScriptableObject
    {
        [field: SerializeField]
        public EnemyController EnemyController { get; set; }

        [field: SerializeField]
        public SimulatedInputDirectionBase SimulatedInputDirection { get; set; }

        /// <summary>
        /// Simulates the time between a player's shots.
        /// </summary>
        /// /// <remarks>
        /// Measured in seconds.
        /// </remarks>
        [field: SerializeField]
        public float PressShootRate { get; set; } = 1f;

        [field: SerializeField]
        public ShipData ShipData { get; set; }
    }
}
