using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "EnemyRound", menuName = "Enemies/EnemyRound")]
    public class EnemyRoundData : ScriptableObject
    {
        [field: SerializeField]
        public float TimeUntilStartSpawn { get; set; } = 1f;

        [field: SerializeField]
        public EnemyControllerData[] EnemiesControllerData { get; set; }

        [field: SerializeField]
        public float TimeBetweenEnemies { get; set; } = 1f;

        [field: SerializeField]
        public float TimeUntilRoundEnds { get; set; } = 1f;

        [field: SerializeField]
        public bool HasBoss { get; set; } = false;
    }
}
