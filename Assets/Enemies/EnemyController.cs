using PowerUps;
using Ships;
using UnityEngine;

namespace Enemies
{
    /// <summary>
    /// 
    /// </summary>
    public class EnemyController : MonoBehaviour
    {
        public delegate void EnemyDied();
        public static event EnemyDied OnEnemyDied;

        [SerializeField]
        private Ship ship;

        [SerializeField]
        private EnemyControllerData enemyControllerData;

        private SimulatedInputDirectionBase simulatedInputDirection;

        /// <summary>
        /// Internal timer for shoot rate/>.
        /// </summary>
        private float pressShootRateTimer;

        [SerializeField]
        private PowerUpData[] powerUpDatas;

        [SerializeField]
        private PowerUp powerUpPrefab;

        [SerializeField]
        [Range(0, 1)]
        private float spawnProbability;

        void Start()
        {
            if (ship != null)
            {
                ship.DamageComponent.OnDeath += () =>
                {
                    OnEnemyDied?.Invoke();

                    float randomSpawnProbability = Random.Range(0f, 1f);
                    if (spawnProbability >= randomSpawnProbability)
                    {
                        SpawnPowerUp();
                    }
                };
            }

            // Preconfigured in editor.
            if (enemyControllerData != null)
            {
                LoadData(enemyControllerData);
            }
        }

        public void LoadData(EnemyControllerData enemyControllerData)
        {
            Debug.Assert(enemyControllerData != null, $"Variable {nameof(enemyControllerData)} cannot be null.");

            this.enemyControllerData = enemyControllerData;

            if (simulatedInputDirection != null)
            {
                Destroy(simulatedInputDirection.gameObject);
            }

            simulatedInputDirection = Instantiate(enemyControllerData.SimulatedInputDirection, transform);
            simulatedInputDirection.transform.SetParent(transform);
        }

        void Update()
        {
            pressShootRateTimer += Time.deltaTime;

            if (ship == null)
            {
                return;
            }

            SimulateMovement();
            SimulateShoot();
        }

        private void SimulateMovement()
        {
            if (simulatedInputDirection == null)
            {
                return;
            }

            var moveDirection = simulatedInputDirection.SimulatePressedDirection();
            ship.Move(moveDirection);
        }

        private void SimulateShoot()
        {
            if (pressShootRateTimer < enemyControllerData.PressShootRate) return;

            pressShootRateTimer = 0;
            ship.ShootLaser();
        }

        /// <summary>
        /// Attach the controller to the ship. Setting the ship as parent.
        /// </summary>
        /// <param name="ship">Ship to be controlled.</param>
        public void TakeShipControl(Ship ship)
        {
            this.ship = ship;
            gameObject.transform.SetParent(this.ship.transform);
        }

        private void SpawnPowerUp()
        {
            int randomIndex = Random.Range(0, powerUpDatas.Length);

            var randomPowerUpData = powerUpDatas[randomIndex];
            var powerUp = Instantiate(powerUpPrefab, ship.transform.position, Quaternion.identity);
            powerUp.LoadData(randomPowerUpData);
        }
    }
}

