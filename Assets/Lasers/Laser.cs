using Movement;
using ObjectsPool;
using UnityEngine;

namespace Lasers
{
    [RequireComponent(typeof(KinematicMovementComponent))]
    public class Laser : MonoBehaviour, IPoolableObject<Laser>
    {
        private SpriteRenderer spriteRenderer;

        [field: SerializeField]
        public int Damage { get; private set; } = 1;

        private MovementComponentBase movementComponent;

        // TODO: Create Timer like Godot?
        public float lifeSpanDuration = 5f;
        private float lifeSpan;

        void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            movementComponent = GetComponent<KinematicMovementComponent>();
        }

        private void OnEnable()
        {
            lifeSpan = 0;
        }

        void Update()
        {
            lifeSpan += Time.deltaTime;
            CheckLifeSpanTimer();
        }

        private void CheckLifeSpanTimer()
        {
            if (lifeSpan >= lifeSpanDuration)
            {
                Destroy();
            }
        }

        public void LoadData(LaserData laserData)
        {
            Debug.Assert(laserData != null, $"Variable {nameof(laserData)} cannot be null.");

            spriteRenderer.sprite = laserData.Sprite;
            movementComponent.Speed = laserData.Speed;
            Damage = laserData.Damage;
            gameObject.tag = laserData.Tag;
        }

        /// <summary>
        /// Disable the gameObject and return to its pool.
        /// </summary>
        public void Destroy()
        {
            lifeSpan = 0;
            ReturnToPool();
        }

        public void ReturnToPool()
        {
            if (isActiveAndEnabled)
            {
                LaserPool.Instance.Release(this);
            }
        }
    }
}