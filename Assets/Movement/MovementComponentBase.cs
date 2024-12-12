using UnityEngine;

namespace Movement
{
    public abstract class MovementComponentBase : MonoBehaviour
    {
        [field: SerializeField]
        public float Speed { get; set; } = 10f;

        [field: SerializeField]
        public Vector2 Direction { get; set; }

        [Header("Movement Bounds")]
        [field: SerializeField]
        public bool LimitMovementBounds;

        // TODO: Hide if LimitMovementBounds is false
        [field: SerializeField]
        public bool LimitToScreenViewport;

        // TODO: Hide if LimitMovementBounds is false
        [field: SerializeField]
        public Bounds MovementBounds { get; set; }

        void Start()
        {
            if (LimitToScreenViewport)
            {
                SetMovementBoundsToScreenViewport();
            }
        }

        void Update()
        {
            Movement();
            LimitMovement();
        }

        protected abstract void Movement();

        private void LimitMovement()
        {
            if (LimitMovementBounds)
            {
                float xClamped = Mathf.Clamp(transform.position.x, MovementBounds.min.x, MovementBounds.max.x);
                float yClamped = Mathf.Clamp(transform.position.y, MovementBounds.min.y, MovementBounds.max.y);
                transform.position = new Vector3(xClamped, yClamped, 0);
            }
        }

        private void SetMovementBoundsToScreenViewport()
        {
            var boundsSize = new Vector3(Camera.main.orthographicSize * Camera.main.aspect * 2, Camera.main.orthographicSize * 2, 0);
            MovementBounds = new Bounds(Camera.main.transform.position, boundsSize);
        }

        public void OnDrawGizmos()
        {
            if (LimitMovementBounds)
            {
                if (LimitToScreenViewport)
                {
                    SetMovementBoundsToScreenViewport();
                }

                var topRight = new Vector3(MovementBounds.max.x, MovementBounds.min.y, 0);
                var bottomLeft = new Vector3(MovementBounds.min.x, MovementBounds.max.y, 0);

                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(MovementBounds.min, topRight);
                Gizmos.DrawLine(topRight, MovementBounds.max);
                Gizmos.DrawLine(MovementBounds.max, bottomLeft);
                Gizmos.DrawLine(bottomLeft, MovementBounds.min);
            }
        }
    }
}
