using UnityEngine;

namespace Bosses
{
    public class BossController : MonoBehaviour
    {
        [SerializeField]
        private Boss boss;

        /// <summary>
        /// Simulates the direction a player wants to move.
        /// </summary>
        [SerializeField]
        private Vector2 PressedDirection = Vector2.left;

        [SerializeField]
        private GameObject stopPoint;

        void Start()
        {
            boss.MovementComponent.Speed = 2; // TODO: Boss initial speed
        }

        void Update()
        {
            SimulateMovement();
        }

        private void SimulateMovement()
        {
            var moveDirection = Vector3.Distance(boss.transform.position, stopPoint.transform.position) > 1f // TODO: Boss stopPoint treshold
                ? PressedDirection : Vector2.zero;
            boss.MovementComponent.Direction = moveDirection;
        }

        public void TakeBossControl(Boss boss)
        {
            this.boss = boss;
        }
    }
}
