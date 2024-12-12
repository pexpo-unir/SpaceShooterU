using System.Collections;
using Bosses;
using ObjectsPool;
using Ships;
using UnityEngine;

namespace Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        public delegate void PlayerWon();
        public static event PlayerWon OnPlayerWon;

        public delegate void RoundChanged(int newRound);
        public static event RoundChanged OnRoundChanged;

        [field: SerializeField]
        public EnemyRoundData[] RoundDatas { get; set; }

        private int currentRound = 0;

        [SerializeField]
        private BossController bossController;

        [SerializeField]
        private Boss bossGameObject;

        [field: SerializeField]
        public Bounds SpawnBounds { get; set; }

        void OnEnable()
        {
            StartCoroutine(RoundCoroutine());
        }

        private IEnumerator RoundCoroutine()
        {
            EnemyRoundData enemyRoundData;
            for (currentRound = 0; currentRound < RoundDatas.Length; currentRound++)
            {
                enemyRoundData = RoundDatas[currentRound];

                OnRoundChanged?.Invoke(currentRound + 1);

                yield return new WaitForSeconds(enemyRoundData.TimeUntilStartSpawn);

                if (enemyRoundData.HasBoss)
                {
                    SpawnBoss();
                }

                yield return StartCoroutine(SpawnEnemyCoroutine(enemyRoundData));

                yield return new WaitForSeconds(enemyRoundData.TimeUntilRoundEnds);
            }

            OnPlayerWon?.Invoke();
        }

        private IEnumerator SpawnEnemyCoroutine(EnemyRoundData enemyRoundData)
        {
            for (int i = 0; i < enemyRoundData.EnemiesControllerData.Length; i++)
            {
                SpawnEnemy(enemyRoundData.EnemiesControllerData[i]);
                yield return new WaitForSeconds(enemyRoundData.TimeBetweenEnemies);
            }
        }

        public void SpawnEnemy(EnemyControllerData enemyControllerData)
        {
            var positionInsideBounds = new Vector3(
                Random.Range(SpawnBounds.min.x, SpawnBounds.max.x),
                Random.Range(SpawnBounds.min.y, SpawnBounds.max.y),
                0);

            var pooledShip = ShipPool.Instance.Get().GetComponent<Ship>();
            pooledShip.transform.SetPositionAndRotation(positionInsideBounds, Quaternion.identity);
            pooledShip.LoadData(enemyControllerData.ShipData);

            var controller = pooledShip.GetComponentInChildren<EnemyController>();
            if (controller == null)
            {
                controller = Instantiate(enemyControllerData.EnemyController, transform.position, Quaternion.identity);
                controller.TakeShipControl(pooledShip);
            }

            controller.LoadData(enemyControllerData);
        }

        public void SpawnBoss()
        {
            var boss = Instantiate(bossGameObject, transform.position, Quaternion.identity);

            var controller = Instantiate(bossController, transform.position, Quaternion.identity);
            controller.TakeBossControl(boss);
        }

        public void OnDrawGizmos()
        {
            var topRight = new Vector3(SpawnBounds.max.x, SpawnBounds.min.y, 0);
            var bottomLeft = new Vector3(SpawnBounds.min.x, SpawnBounds.max.y, 0);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(SpawnBounds.min, topRight);
            Gizmos.DrawLine(topRight, SpawnBounds.max);
            Gizmos.DrawLine(SpawnBounds.max, bottomLeft);
            Gizmos.DrawLine(bottomLeft, SpawnBounds.min);
        }
    }
}
